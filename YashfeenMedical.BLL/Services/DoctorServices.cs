using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Doctors;
using YashfeenMedical.BLL.DTOs.DoctorSchedules;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.DAL.Repositories;
using YashfeenMedical.Infrastructure.Exceptions;
using YashfeenMedical.Infrastructure.FileStorage;
using YashfeenMedical.Infrastructure.UsersManagment;
using Microsoft.EntityFrameworkCore;
using dal = YashfeenMedical.DAL;
using YashfeenMedical.BLL.DTOs.Appointments;

namespace YashfeenMedical.BLL.Services
{
    public class DoctorServices : TEntityService<Doctor, int, DoctorDto, DoctorCreationDto, DoctorUpdateDto>, IDoctorServices
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserManagmentServices _userManagmentServices;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IPaginationServices _paginationServices;

        public DoctorServices(IDoctorRepository repository, IMapper mapper
            , IUserManagmentServices userManagmentServices, IFileStorageService fileStorageService
            , IUnitOfWork unitOfWork, IPaginationServices paginationServices) : base(repository, mapper, paginationServices)
        {
            _repository = repository;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _userManagmentServices = userManagmentServices;
            _unitOfWork = unitOfWork;
            _paginationServices = paginationServices;
        }

        public async Task<TPaginationQueryModel<DoctorDto>> GetFilteredDoctorsWithPaginationAsync(DoctorQueryModel queryModel)
        {
            var doctors = _repository.GetFilteredDoctorsAsync(queryModel);
            var doctorsDtos = doctors.ProjectToType<DoctorDto>();

            var paggedList = await _paginationServices.GetPaggedList(doctorsDtos, queryModel);

            return paggedList;
        }

        public async override Task<DoctorDto> Add(DoctorCreationDto creationDto)
        {
            await CheckUserInformation(
                creationDto.UserName,
                creationDto.Email);

            await _unitOfWork.BeginTransactionAsync();

            string? profilePicturePath = null;

            try
            {
                var user = await CreateDoctorUserAsync(creationDto);

                var doctor = await CreateDoctorAsync(creationDto, user, profilePicturePath);

                await _unitOfWork.Doctors.Add(doctor);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return MapDoctorToDto(doctor, profilePicturePath);
            }
            catch (AppException)
            {
                await RollbackAction(profilePicturePath);
                throw;
            }
            catch (Exception ex)
            {
                await RollbackAction(profilePicturePath);

                throw new InternalServerErorrException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<TPaginationQueryModel<DoctorScheduleDto>> GetDoctorSchedule(int doctorId, PaginationQuery paginationQuery)
        {
            await IsExists(doctorId);

            var schedule = _unitOfWork.DoctorSchedules.GetDoctorScheduleAsync(doctorId);
            var scheduleDtos = schedule.ProjectToType<DoctorScheduleDto>();

            var paggedList = await _paginationServices.GetPaggedList(scheduleDtos, paginationQuery);

            return paggedList;
        }

        public async Task<List<AvailableSlotDto>> GetDoctorScheduleOnDayAsync(int doctorId, DateOnly date)
        {
            CheckDate(date);

            await IsExists(doctorId);

            var schedule = await CheckActiveSchedule(doctorId, date);
            var bookedTimes = await GetBookedTimes(doctorId, date);

            CheckMaxAppointmentsPerDay(bookedTimes, schedule);

            var bookedSet = bookedTimes.ToHashSet();



            var availableSlots = GetAvailableSlots(bookedSet, schedule);

            return SetAvailableSlotsDateRange(availableSlots, date);
        }

        public async Task<TPaginationQueryModel<AppointmentDto>> GetDoctorAppointments(int doctorId, PaginationQuery paginationQuery)
        {
            var doctor = await Details(doctorId);

            var appointment = _unitOfWork.Appointments.GetDoctorAppointments(doctorId);
            var appointmentDtos = appointment.ProjectToType<AppointmentDto>();

            var paggedList = await _paginationServices.GetPaggedList(appointmentDtos, paginationQuery);

            return paggedList;
        }

        public async Task<DoctorScheduleDto> UpsertSchedule(int doctorId, DoctorScheduleUpdateDto doctorSchedule)
        {
            await IsExists(doctorId);

            var schdeule = await _unitOfWork.DoctorSchedules.GetById(doctorSchedule.Id);

            if (schdeule == null)
            {
                var newSchedule = _mapper.Map<DoctorSchedule>(doctorSchedule);
                newSchedule.DoctorId = doctorId;
                await _unitOfWork.DoctorSchedules.Add(newSchedule);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<DoctorScheduleDto>(newSchedule);
            }
            else
            {
                await _unitOfWork.DoctorSchedules.Update(schdeule);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<DoctorScheduleDto>(schdeule);
            }
            ;
        }

        public async Task<string> TogglePatientActivitiy(int doctorId)
        {
            var doctor = await _repository.GetById(doctorId);

            if (doctor.IsAvailable == false)
            {
                doctor.IsAvailable = true;
                await _repository.Update(doctor);
                return "doctor is now available";
            }
            else
            {
                doctor.IsAvailable = false;
                await _repository.Update(doctor);
                return "doctor is now unavailable";
            }
        }

        public async Task<bool> UploadPatientPhoto(int doctorId, IFormFile ProfilePhoto)
        {

            {
                var doctor = await _repository.GetById(doctorId)
                    ?? throw new NotFoundException("The request entity dosen't exits");

                var oldPhotoPath = doctor.ProfilePhotoUrl;

                string? profilePicturePath = null;

                try
                {
                    if (ProfilePhoto != null)
                    {
                        profilePicturePath = await SetProfilePhoto(doctor, ProfilePhoto);
                    }

                    await _repository.Update(doctor);

                    if (profilePicturePath != null && !string.IsNullOrWhiteSpace(oldPhotoPath))
                    {
                        _fileStorageService.DeleteFile(oldPhotoPath);
                    }

                    return true;
                }

                catch (AppException)
                {
                    if (profilePicturePath != null)
                        _fileStorageService.DeleteFile(profilePicturePath);

                    throw;
                }

                catch (Exception ex)
                {

                    if (profilePicturePath != null)
                        _fileStorageService.DeleteFile(profilePicturePath);

                    throw new Exception("Error occurred while uploading doctor photo.", ex);
                }

            }
        }

        public async override Task<DoctorDto> Update(int id, DoctorUpdateDto updateDto)
        {
            var doctor = await _repository.GetById(id);

            if (doctor == null)
                throw new NotFoundException("The request entity dosen't exits");

            var oldProfilePicturePath = doctor.ProfilePhotoUrl;
            string? newPofilePicturePath = null;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (updateDto.ProfilePhoto != null)
                {
                    newPofilePicturePath = await SetProfilePhoto(doctor, updateDto.ProfilePhoto);
                }

                var mappedEntity = _mapper.Map(updateDto, doctor);

                var user = await _userManagmentServices.FindUserAsync(mappedEntity.UserId);

                await SetUserName(user, updateDto.UserName);
                await SetEmail(user, updateDto.Email);
                await SetPhoneNumber(user, updateDto.PhoneNumber);

                mappedEntity.UpdatedOn = DateTimeOffset.UtcNow;

                await _unitOfWork.Doctors.Update(mappedEntity);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                var result = _mapper.Map<DoctorDto>(mappedEntity);

                if (newPofilePicturePath != null && !string.IsNullOrWhiteSpace(oldProfilePicturePath))
                {
                    _fileStorageService.DeleteFile(oldProfilePicturePath);
                }

                if (newPofilePicturePath != null)
                    result.ProfilePhotoUrl = _fileStorageService.GenerateSignedUrl(newPofilePicturePath, TimeSpan.FromHours(1));

                return result;
            }

            catch (AppException)
            {
                throw;
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                if (newPofilePicturePath != null)
                    _fileStorageService.DeleteFile(newPofilePicturePath);

                throw new Exception("Error occurred while saving the patient.", ex);
            }
        }

        public async override Task Delete(int id)
        {
            var doctor = await _repository.GetById(id);

            if (doctor == null)
                throw new NotFoundException("The request entity dosen't exits");

            var user = await _userManagmentServices.FindUserAsync(doctor.UserId);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                user.DeletedOn = DateTimeOffset.UtcNow;
                user.IsActive = false;
                await _userManagmentServices.UpdateUserAsync(user);
                await _repository.Delete(id);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Error occurred while deleting the patient.", ex);
            }

        }

        private async Task<string?> SetProfilePhoto(Doctor patient, IFormFile profilePhoto)
        {
            var oldPhotoPath = patient.ProfilePhotoUrl;
            string? newPhotoPath = null;

            if (profilePhoto != null)
                newPhotoPath = await _fileStorageService.SaveProfilePhoto(profilePhoto, "doctors");

            patient.ProfilePhotoUrl = newPhotoPath ?? oldPhotoPath;

            return newPhotoPath;
        }
        private async Task RollbackAction(string? profilePicturePath)
        {
            await _unitOfWork.RollbackTransactionAsync();

            if (!string.IsNullOrWhiteSpace(profilePicturePath))
            {
                _fileStorageService.DeleteFile(profilePicturePath);
            }
        }
        private async Task CheckUserInformation(string name, string email)
        {
            var userName = await _userManagmentServices.FindUserByNameAsync(name);

            if (userName != null)
                throw new BadRequestException("this username is used by another user");

            var userEmail = await _userManagmentServices.FindUserByEmailAsync(email);

            if (userEmail != null)
                throw new BadRequestException("this email is used by another user");
        }
        private async Task<IList<Specialty>> GetDoctorSpecialties(IList<int> ids)
        {
            var specialties = new List<Specialty>();

            foreach (var id in ids)
            {
                var specialty = await _unitOfWork.Specialties.GetById(id)
                    ?? throw new NotFoundException($"Specialty with id {id} not found");

                specialties.Add(specialty);
            }

            return specialties;
        }
        private async Task<ApplicationUser> CreateDoctorUserAsync(DoctorCreationDto creationDto)
        {
            var user = new ApplicationUser
            {
                UserName = creationDto.UserName,
                Email = creationDto.Email,
                IsActive = true,
                CreatedOn = DateTimeOffset.UtcNow,
                PhoneNumber = creationDto.PhoneNumber
            };

            var result = await _userManagmentServices
                .CreateUserAsync(user, creationDto.Password);

            if (!result.Succeeded)
            {
                throw new InternalServerErorrException(
                    string.Join(", ",
                        result.Errors.Select(e => e.Description)));
            }

            await _userManagmentServices.AddUserToRole(
                user,
                "Doctor");

            return user;
        }
        private async Task<Doctor> CreateDoctorAsync(DoctorCreationDto creationDto, ApplicationUser user, string? profilePicturePath)
        {
            var doctor = _mapper.Map<Doctor>(creationDto);

            profilePicturePath = null;

            if (creationDto.ProfilePhoto != null)
            {
                profilePicturePath = await SetProfilePhoto(
                    doctor,
                    creationDto.ProfilePhoto);
            }

            doctor.UserId = user.Id;
            doctor.ProfilePhotoUrl = profilePicturePath;

            doctor.Specialties =
                await GetDoctorSpecialties(creationDto.Specialties);

            return doctor;
        }
        private DoctorDto MapDoctorToDto(Doctor doctor, string? profilePicturePath)
        {
            var result = _mapper.Map<DoctorDto>(doctor);

            if (!string.IsNullOrWhiteSpace(profilePicturePath))
            {
                result.ProfilePhotoUrl =
                    _fileStorageService.GenerateSignedUrl(
                        profilePicturePath,
                        TimeSpan.FromHours(1));
            }

            return result;
        }

        private async Task<DoctorSchedule> CheckActiveSchedule(int doctorId, DateOnly date)
        {
            var dayOfWeek = (ScheduleDayOfWeek)date.DayOfWeek;

            var schedule = await _unitOfWork.DoctorSchedules
                .GetByDoctorAndDayAsync(doctorId, dayOfWeek);
            if (schedule == null)
                throw new NotFoundException($"No schedule found for doctor with id {doctorId} on {dayOfWeek}");

            if (!schedule.IsActive)
                throw new BadRequestException($"The schedule for doctor with id {doctorId} on {dayOfWeek} is not active");

            return schedule;
        }
        private void CheckDate(DateOnly date)
        {
            var today = DateOnly.FromDateTime(DateTime.Now.Date);

            if (date < today)
                throw new BadRequestException(
                    "Cannot display available times for a past date.");
        }
        private async Task<List<TimeOnly>> GetBookedTimes(int doctorId, DateOnly date)
        {
            var appointments = _unitOfWork.Appointments.GetAll();

            var bookedStatuses = new[]
           {
             AppointmentStatus.Pending,
             AppointmentStatus.Confirmed,
             AppointmentStatus.InProgress
            };


            var bookedTimes = await appointments.Where(a => a.DoctorId == doctorId
           && a.AppointmentDate == date
           && bookedStatuses.Contains(a.Status))
             .Select(a => a.StartTime)
             .ToListAsync();

            return bookedTimes;
        }
        private List<AvailableSlotDto> GetAvailableSlots(HashSet<TimeOnly> bookedTimes, DoctorSchedule schedule)
        {
            var result = new List<AvailableSlotDto>();

            var current = schedule.StartTime;

            while (current.AddMinutes(schedule.SlotDurationMinutes)
                          <= schedule.EndTime)
            {
                if (!bookedTimes.Contains(current))
                {
                    result.Add(new AvailableSlotDto
                    {
                        StartTime = current,
                        EndTime = current.AddMinutes(
                            schedule.SlotDurationMinutes),
                        IsAvailable = true
                    });
                }

                current = current.AddMinutes(
                    schedule.SlotDurationMinutes);
            }

            return result;
        }
        private List<AvailableSlotDto> SetAvailableSlotsDateRange(List<AvailableSlotDto> availableSlots, DateOnly date)
        {
            if (date == DateOnly.FromDateTime(DateTime.Now.Date))
            {
                var now = TimeOnly.FromDateTime(DateTime.Now);
                availableSlots = availableSlots.Where(s => s.StartTime > now).ToList();
            }

            return availableSlots;
        }
        private void CheckMaxAppointmentsPerDay(List<TimeOnly> bookedTimes, DoctorSchedule schedule)
        {
            if (bookedTimes.Count >= schedule.MaxAppointmentsPerDay)
                throw new BadRequestException("this doctor has reached the Max Appointments Per Day");
        }

        private async Task SetUserName(ApplicationUser user, string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName) && userName != user.UserName)
            {
                var nameExists = await _userManagmentServices.FindUserByNameAsync(userName);
                if (nameExists != null && nameExists.Id != user.Id)
                    throw new ConflictException("this username is already in use");

                var nameResult = await _userManagmentServices.SetUserNameAsync(user, userName);
                if (!nameResult.Succeeded)
                    throw new BadRequestException(string.Join(", ", nameResult.Errors.Select(e => e.Description)));
            }
        }

        private async Task SetPhoneNumber(ApplicationUser user, string phoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber != user.PhoneNumber)
            {
                var phoneResult = await _userManagmentServices.SetPhoneNumberAsync(user, phoneNumber);
                if (!phoneResult.Succeeded)
                    throw new BadRequestException(string.Join(", ", phoneResult.Errors.Select(e => e.Description)));
            }
        }

        private async Task SetEmail(ApplicationUser user, string email)
        {
            if (!string.IsNullOrWhiteSpace(email) && email != user.Email)
            {
                var emailExists = await _userManagmentServices.FindUserByEmailAsync(email);
                if (emailExists != null && emailExists.Id != user.Id)
                    throw new ConflictException("this email is already in use");
                var emailResult = await _userManagmentServices.SetUserEmailAsync(user, email);

                if (!emailResult.Succeeded)
                    throw new BadRequestException(string.Join(", ", emailResult.Errors.Select(e => e.Description)));
            }
        }
    }
}

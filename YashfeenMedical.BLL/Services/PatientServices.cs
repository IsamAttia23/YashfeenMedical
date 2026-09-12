using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.Invoices;
using YashfeenMedical.BLL.DTOs.MedicalFiles;
using YashfeenMedical.BLL.DTOs.MedicalRecords;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.DTOs.Prescriptions;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.Infrastructure.Exceptions;
using YashfeenMedical.Infrastructure.FileStorage;
using YashfeenMedical.Infrastructure.UsersManagment;

namespace YashfeenMedical.BLL.Services
{
    public class PatientServices : TEntityService<Patient, int, PatientDto, PatientCreationDto, PatientUpdateDto>, IPatientServices
    {
        private readonly IPatientRepository _repository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMedicalFileRepository _medicalFileRepository;
        private readonly IUserManagmentServices _userManagmentServices;
        private readonly IFileStorageService _fileStorageService;
        private readonly IPaginationServices _paginationServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientServices(IPatientRepository repository, IMapper mapper
            , IAppointmentRepository appointmentRepository, IMedicalRecordRepository medicalRecordRepository,
              IPrescriptionRepository prescriptionRepository, IInvoiceRepository invoiceRepository,
              IMedicalFileRepository medicalFileRepository, IUserManagmentServices userManagmentServices,
              IFileStorageService fileStorageService, IUnitOfWork unitOfWork,
              IPaginationServices pagination) : base(repository, mapper)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _prescriptionRepository = prescriptionRepository;
            _invoiceRepository = invoiceRepository;
            _medicalFileRepository = medicalFileRepository;
            _userManagmentServices = userManagmentServices;
            _paginationServices = pagination;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TPaginationQueryModel<PatientDto>> GetFilterdPatients(PatientQueryModel queryModel)
        {
            var patients = await _repository.GetFilteredPatientsAsync(queryModel);
            var mappedPatients = patients.ProjectToType<PatientDto>();

            var paggedList = await _paginationServices.GetPaggedList(mappedPatients, queryModel);

            return paggedList;
        }

        public async Task<TPaginationQueryModel<InvoiceDto>> GetPaitentInvoices(PaginationQuery queryModel, int paitentId)
        {
            var invoices = await _invoiceRepository.GetInvoicesByPatientId(paitentId);
            var mappedInvoices = invoices.ProjectToType<InvoiceDto>();

            var paggedList = await _paginationServices.GetPaggedList(mappedInvoices, queryModel);

            return paggedList;
        }

        public async Task<TPaginationQueryModel<AppointmentDto>> GetPaitentAppointments(PatientAppointmentsQueryModel queryModel, int paitentId)
        {
            var patientAppointments = _appointmentRepository.GetPatientAppointmentsAsync(paitentId);
            var filterdAppointments = _appointmentRepository.GetFilterdAppointmentsAsync(queryModel, patientAppointments);
            var mappedAppointments = filterdAppointments.ProjectToType<AppointmentDto>();

            var paggedList = await _paginationServices.GetPaggedList(mappedAppointments, queryModel);

            return paggedList;
        }

        public async Task<TPaginationQueryModel<MedicalRecordDto>> GetPaitentMedicalRecords(PaginationQuery queryModel, int paitentId)
        {
            var medicalRecords = await _medicalRecordRepository.GetByPatientId(paitentId);
            var mappedMedicalRecords = medicalRecords.ProjectToType<MedicalRecordDto>();

            var paggedList = await _paginationServices.GetPaggedList(mappedMedicalRecords, queryModel);

            return paggedList;
        }

        public async Task<TPaginationQueryModel<MedicalFileDto>> GetPaitentMedicalFiles(PaginationQuery queryModel, int paitentId)
        {
            var medicalFiles = await _medicalFileRepository.GetMedicalFileByPatientId(paitentId);
            var mappedMedicalFiles = medicalFiles.ProjectToType<MedicalFileDto>();

            var paggedList = await _paginationServices.GetPaggedList(mappedMedicalFiles, queryModel);

            return paggedList;
        }

        public async Task<TPaginationQueryModel<PrescriptionDto>> GetPaitentPrescriptions(PaginationQuery queryModel, int paitentId)
        {
            var prescriptions = await _prescriptionRepository.GetPrescriptionsByPatientId(paitentId);
            var mappedPrescriptions = prescriptions.ProjectToType<PrescriptionDto>();

            var paggedList = await _paginationServices.GetPaggedList(mappedPrescriptions, queryModel);

            return paggedList;
        }

        public async Task<string> TogglePatientActivitiy(int patientId)
        {
            var patient = await _repository.GetById(patientId);
            var user = await _userManagmentServices.FindUserAsync(patient.UserId);

            if (user.IsActive == false)
            {
                user.IsActive = true;
                await _userManagmentServices.UpdateUserAsync(user);
                return "patient activated successfully";
            }
            else
            {
                user.IsActive = false;
                await _userManagmentServices.UpdateUserAsync(user);
                return "patient deactivated successfully";
            }
        }

        public async Task<bool> UploadPatientPhoto(int patientId, IFormFile ProfilePhoto)
        {
            var patient = await _repository.GetById(patientId)
                ?? throw new NotFoundException("The request entity dosen't exits");

            var oldPhotoPath = patient.ProfilePhotoUrl;

            string? profilePicturePath = null;

            try
            {
                if (ProfilePhoto != null)
                {
                    profilePicturePath = await SetProfilePhoto(patient, ProfilePhoto);
                }

                await _repository.Update(patient);

                if (profilePicturePath != null && !string.IsNullOrWhiteSpace(oldPhotoPath))
                {
                    _fileStorageService.DeleteFile(oldPhotoPath);
                }

                return true;
            }

            catch (AppException)
            {
                throw;
            }

            catch (Exception ex)
            {

                if (profilePicturePath != null)
                    _fileStorageService.DeleteFile(profilePicturePath);

                throw new Exception("Error occurred while uploading patient photo.", ex);
            }

        }

        public async override Task<PatientDto> Update(int id, PatientUpdateDto updateDto)
        {
            var patient = await _repository.GetById(id);

            if (patient == null)
                throw new NotFoundException("The request entity dosen't exits");

            var oldProfilePicturePath = patient.ProfilePhotoUrl;
            string? newPofilePicturePath = null;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (updateDto.ProfilePhoto != null)
                {
                    newPofilePicturePath = await SetProfilePhoto(patient, updateDto.ProfilePhoto);
                }

                var mappedEntity = _mapper.Map(updateDto, patient);

                var user = await _userManagmentServices.FindUserAsync(mappedEntity.UserId);

                await SetUserName(user, updateDto.UserName);
                await SetEmail(user, updateDto.Email);
                await SetPhoneNumber(user, updateDto.PhoneNumber);

                mappedEntity.UpdatedOn = DateTimeOffset.UtcNow;

                await _unitOfWork.Patients.Update(mappedEntity);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                var result = _mapper.Map<PatientDto>(mappedEntity);

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
            var patient = await _repository.GetById(id);

            if (patient == null)
                throw new NotFoundException("The request entity dosen't exits");

            var user = await _userManagmentServices.FindUserAsync(patient.UserId);
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

        private async Task<string?> SetProfilePhoto(Patient patient, IFormFile profilePhoto)
        {
            var oldPhotoPath = patient.ProfilePhotoUrl;
            string? newPhotoPath = null;

            if (profilePhoto != null)
                newPhotoPath = await _fileStorageService.SaveProfilePhoto(profilePhoto, "patients");

            patient.ProfilePhotoUrl = newPhotoPath ?? oldPhotoPath;

            return newPhotoPath;
        }

    }
}

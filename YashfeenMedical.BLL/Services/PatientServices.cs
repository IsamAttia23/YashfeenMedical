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
        private readonly IMapper _mapper;

        public PatientServices(IPatientRepository repository, IMapper mapper
            , IAppointmentRepository appointmentRepository, IMedicalRecordRepository medicalRecordRepository,
              IPrescriptionRepository prescriptionRepository, IInvoiceRepository invoiceRepository,
              IMedicalFileRepository medicalFileRepository, IUserManagmentServices userManagmentServices,
              IFileStorageService fileStorageService) : base(repository, mapper)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _prescriptionRepository = prescriptionRepository;
            _invoiceRepository = invoiceRepository;
            _medicalFileRepository = medicalFileRepository;
            _userManagmentServices = userManagmentServices;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<TPaginationQueryModel<PatientDto>> GetFilterdPatients(PatientQueryModel queryModel)
        {
            var paggedPatients = await _repository.GetFilteredPatientsWithPaginationAsync(queryModel);

            var result = _mapper.Map<TPaginationQueryModel<PatientDto>>(paggedPatients);

            return result;
        }

        public async Task<TPaginationQueryModel<InvoiceDto>> GetPaitentInvoices(PaginationQuery queryModel, int paitentId)
        {
            var invoices = await _invoiceRepository.GetInvoicesByPatientId(paitentId);
            var paggedList = await _invoiceRepository.GetPaggedList(invoices, queryModel);
            var result = _mapper.Map<TPaginationQueryModel<InvoiceDto>>(paggedList);

            return result;
        }

        public async Task<TPaginationQueryModel<AppointmentDto>> GetPaitentAppointments(PatientAppointmentsQueryModel queryModel, int paitentId)
        {
            var patientAppointments = await _appointmentRepository.GetPatientAppointmentsAsync(paitentId);
            var filterdAppointments = await _appointmentRepository.GetFilterdAppointmentsAsync(queryModel, patientAppointments);
            var paggedList = await _appointmentRepository.GetPaggedList(filterdAppointments, queryModel);
            var result = _mapper.Map<TPaginationQueryModel<AppointmentDto>>(paggedList);

            return result;
        }

        public async Task<TPaginationQueryModel<MedicalRecordDto>> GetPaitentMedicalRecords(PaginationQuery queryModel, int paitentId)
        {
            var medicalRecords = await _medicalRecordRepository.GetByPatientId(paitentId);
            var paggedList = await _medicalRecordRepository.GetPaggedList(medicalRecords, queryModel);
            var result = _mapper.Map<TPaginationQueryModel<MedicalRecordDto>>(paggedList);

            return result;
        }

        public async Task<TPaginationQueryModel<MedicalFileDto>> GetPaitentMedicalFiles(PaginationQuery queryModel, int paitentId)
        {
            var medicalFiles = await _medicalFileRepository.GetMedicalFileByPatientId(paitentId);
            var paggedList = await _medicalFileRepository.GetPaggedList(medicalFiles, queryModel);
            var result = _mapper.Map<TPaginationQueryModel<MedicalFileDto>>(paggedList);

            return result;
        }

        public async Task<TPaginationQueryModel<PrescriptionDto>> GetPaitentPrescriptions(PaginationQuery queryModel, int paitentId)
        {
            var prescriptions = await _prescriptionRepository.GetPrescriptionsByPatientId(paitentId);
            var paggedList = await _prescriptionRepository.GetPaggedList(prescriptions, queryModel);
            var result = _mapper.Map<TPaginationQueryModel<PrescriptionDto>>(paggedList);

            return result;
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
            var patient = await _repository.GetById(patientId);

            string? profilePicturePath = null;

            if (ProfilePhoto != null)
            {
                profilePicturePath = await _fileStorageService.SaveProfilePhoto(ProfilePhoto);
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(patient.ProfilePhotoUrl))
                {
                    _fileStorageService.DeleteFile(patient.ProfilePhotoUrl);
                }

                patient.ProfilePhotoUrl = profilePicturePath;
                await _repository.Update(patient);

                return true;
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

            string? profilePicturePath = null;

            if (updateDto.ProfilePhoto != null)
            {
                profilePicturePath = await _fileStorageService.SaveProfilePhoto(updateDto.ProfilePhoto);
                updateDto.ProfilePhotoUrl = profilePicturePath;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(patient.ProfilePhotoUrl))
                {
                    _fileStorageService.DeleteFile(patient.ProfilePhotoUrl);
                }

                var mappedEntity = _mapper.Map(updateDto, patient);
                var user = await _userManagmentServices.FindUserAsync(mappedEntity.UserId);

                await SetUserName(user, updateDto.UserName);
                await SetEmail(user, updateDto.Email);
                await SetPhoneNumber(user, updateDto.PhoneNumber);

                mappedEntity.UpdatedOn = DateTimeOffset.UtcNow;

                await _repository.Update(mappedEntity);
                await _repository.SaveChanges();

                var result = _mapper.Map<PatientDto>(mappedEntity);

                if (profilePicturePath != null)
                    result.ProfilePhotoUrl = _fileStorageService.GenerateSignedUrl(profilePicturePath, TimeSpan.FromHours(1));
               
                return result;
            }
            catch (Exception ex)
            {

                if (profilePicturePath != null)
                    _fileStorageService.DeleteFile(profilePicturePath);

                throw new Exception("Error occurred while saving the patient.", ex);
            }

        }

        private async Task SetUserName(ApplicationUser user,string userName)
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

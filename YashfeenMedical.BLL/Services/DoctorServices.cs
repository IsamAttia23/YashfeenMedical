using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Doctors;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.Infrastructure.Exceptions;
using YashfeenMedical.Infrastructure.FileStorage;
using YashfeenMedical.Infrastructure.UsersManagment;

namespace YashfeenMedical.BLL.Services
{
    public class DoctorServices : TEntityService<Doctor, int, DoctorDto, DoctorCreationDto, DoctorUpdateDto>, IDoctorServices
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserManagmentServices _userManagmentServices;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorServices(IDoctorRepository repository, IMapper mapper
            , IUserManagmentServices userManagmentServices, IFileStorageService fileStorageService
            , IUnitOfWork unitOfWork) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _userManagmentServices = userManagmentServices;
            _unitOfWork = unitOfWork;
        }

        public async Task<TPaginationQueryModel<DoctorDto>> GetFilteredDoctorsWithPaginationAsync(DoctorQueryModel queryModel)
        {
            var doctors = _repository.GetFilteredDoctorsAsync(queryModel);
            var doctorsDtos = doctors.ProjectToType<DoctorDto>();
            var paginatedDoctors = await GetPaggedList(doctorsDtos, queryModel);

            var result = _mapper.Map<TPaginationQueryModel<DoctorDto>>(paginatedDoctors);

            return result;
        }

        public async override Task<DoctorDto> Add(DoctorCreationDto creationDto)
        {
            var userName = await _userManagmentServices.FindUserByNameAsync(creationDto.UserName);

            if (userName != null)
                throw new BadRequestException("this username is used by another user");

            var userEmail = await _userManagmentServices.FindUserByEmailAsync(creationDto.Email);

            if (userEmail != null)
                throw new BadRequestException("this email is used by another user");

            var mappedDoctor = _mapper.Map<Doctor>(creationDto);

            string? profilePicturePath = null;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (creationDto.ProfilePhoto != null)
                {
                    profilePicturePath = await SetProfilePhoto(mappedDoctor, creationDto.ProfilePhoto);
                }

                var user = new ApplicationUser
                {
                    UserName = creationDto.UserName,
                    Email = creationDto.Email,
                    IsActive = true,
                    CreatedOn = DateTimeOffset.UtcNow,
                    PhoneNumber = creationDto.PhoneNumber,

                };

                var createResult = await _userManagmentServices.CreateUserAsync(user, creationDto.Password);
                if (!createResult.Succeeded)
                {
                    throw new InternalServerErorrException(
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }

                await _userManagmentServices.AddUserToRole(user, "Doctor");


                mappedDoctor.UserId = user.Id;
                mappedDoctor.ProfilePhotoUrl = profilePicturePath;

                await _unitOfWork.Doctors.Add(mappedDoctor);
                await _unitOfWork.SaveChangesAsync();



                var result = _mapper.Map<DoctorDto>(mappedDoctor);

                if (profilePicturePath != null)
                    result.ProfilePhotoUrl = _fileStorageService.GenerateSignedUrl(profilePicturePath, TimeSpan.FromHours(1));

                await _unitOfWork.CommitTransactionAsync();

                return result;
            }
            catch (AppException)
            {
                await RollbackAction(profilePicturePath);
                throw;
            }

            catch (Exception ex)
            {

                await _unitOfWork.RollbackTransactionAsync();

                if (profilePicturePath != null)
                    _fileStorageService.DeleteFile(profilePicturePath);

                throw new InternalServerErorrException(
                ex.InnerException?.Message ?? ex.Message);
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

        private async Task RollbackAction(string? photoPath)
        {
            await _unitOfWork.RollbackTransactionAsync();

            if (photoPath != null)
                _fileStorageService.DeleteFile(photoPath);
        }
    }
}

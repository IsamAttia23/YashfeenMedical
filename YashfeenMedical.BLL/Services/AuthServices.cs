using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using YashfeenMedical.BLL.DTOs.Auth;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.Infrastructure.Exceptions;
using YashfeenMedical.Infrastructure.JWT;
using YashfeenMedical.Infrastructure.UsersManagment;
using Microsoft.EntityFrameworkCore;
using YashfeenMedical.Infrastructure.FileStorage;

namespace YashfeenMedical.BLL.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IJwtService _jwtService;
        private readonly IUserManagmentServices _userManagmentServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public AuthServices(IJwtService jwtService, IUserManagmentServices userManagmentServices
            , IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _jwtService = jwtService;
            _userManagmentServices = userManagmentServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        private async Task<RefreshToken> AssignRefreshTokenToUser(ApplicationUser user, AuthDto authDto)
        {
            var refreshToken = new RefreshToken();
            if (user.RefreshTokens.Any(t => t.IsActive is true))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive is true);
                authDto.RefreshToken = activeRefreshToken.Token;
                authDto.RefreshTokenExpiration = activeRefreshToken.ExpireOn;
            }
            else
            {
                refreshToken.Token = _jwtService.GenerateRefreshToken();
                refreshToken.CreatedOn = DateTimeOffset.UtcNow;
                refreshToken.ExpireOn = _jwtService.GetRefreshTokenExpiry();
                authDto.RefreshToken = refreshToken.Token;
                authDto.RefreshTokenExpiration = refreshToken.ExpireOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManagmentServices.UpdateUserAsync(user);
            }

            return refreshToken;
        }

        public async Task ChangePasswordAsync(ChangePasswordDto passwordDto)
        {
            var user = await _userManagmentServices.FindUserAsync(passwordDto.UserId);

            var result = await _userManagmentServices.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);
            if (!result.Succeeded)
                throw new BadRequestException($"Can't change password :{result.Errors} ");
        }

        public async Task<AuthDto> LoginAsync(LoginDto loginDto)
        {
            var authDto = new AuthDto();
            var user = await _userManagmentServices.FindUserByEmailAsync(loginDto.Email);

            if (user is null || !await _userManagmentServices.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new NotFoundException("incorrect user or password");
            }

            var rolesList = await _userManagmentServices.GetUserRoles(user);
            var jwtSecurityToken = await _jwtService.GenerateAccessToken(user);

            authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authDto.Email = loginDto.Email;
            authDto.UserName = user.UserName;
            authDto.Roles = rolesList.ToList();

            await AssignRefreshTokenToUser(user, authDto);

            return authDto;
        }

        public async Task LogoutAsync(string token)
        {
            var user = await _userManagmentServices.CheckRefreshTokenAsync(token);

            if (user is null)
                throw new NotFoundException("invalid refresh token");

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (refreshToken.IsActive == false)
                throw new NotFoundException("invalid refresh token");

            refreshToken.RevokedOn = DateTimeOffset.UtcNow;
            await _userManagmentServices.UpdateUserAsync(user);

        }

        public async Task<AuthDto> RefreshTokenAsync(string token)
        {
            var authDto = new AuthDto();
            var user = await _userManagmentServices.CheckRefreshTokenAsync(token);

            if (user is null)
                throw new NotFoundException("invalid refresh token");

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (refreshToken.IsActive == false)
                throw new NotFoundException("invalid refresh token");

            refreshToken.RevokedOn = DateTimeOffset.UtcNow;

            var newRefreshToken = await AssignRefreshTokenToUser(user, authDto);

            authDto.Email = user.Email;
            authDto.RefreshToken = newRefreshToken.Token;

            return authDto;
        }

        public async Task<PatientDto> RegisterPatientAsync(PatientCreationDto creationDto)
        {
            var userName = await _userManagmentServices.FindUserByNameAsync(creationDto.UserName);
            if (userName != null)
                throw new BadRequestException("this username is used by another user");

            var userEmail = await _userManagmentServices.FindUserByEmailAsync(creationDto.Email);
            if (userEmail != null)
                throw new BadRequestException("this email is used by another user");

            var mappedPatient = _mapper.Map<Patient>(creationDto);

            string? profilePicturePath = null;
            if (creationDto.ProfilePicture != null)
            {

                profilePicturePath = await _fileStorageService.SaveProfilePhoto(creationDto.ProfilePicture);
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
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

                await _userManagmentServices.AddUserToRole(user, "Patient");


                mappedPatient.UserId = user.Id;
                mappedPatient.ProfilePhotoUrl = profilePicturePath;

                await _unitOfWork.Patients.Add(mappedPatient);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }

            catch (Exception ex)
            {

                await _unitOfWork.RollbackTransactionAsync();

                if (profilePicturePath != null)
                    _fileStorageService.DeleteFile(profilePicturePath);

                throw new InternalServerErorrException(
                ex.InnerException?.Message ?? ex.Message);
            }

            var result = _mapper.Map<PatientDto>(mappedPatient);
            if (profilePicturePath != null)
                result.ProfilePhotoUrl = _fileStorageService.GenerateSignedUrl(profilePicturePath, TimeSpan.FromHours(1));

            return result;
        }
    }
}

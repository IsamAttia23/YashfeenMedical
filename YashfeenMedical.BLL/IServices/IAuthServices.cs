using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using YashfeenMedical.BLL.DTOs.Auth;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.DTOs.Users;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.BLL.IServices
{
    public interface IAuthServices
    {
        Task<AuthDto> LoginAsync(LoginDto loginDto);
        Task<RefreshToken> AssignRefreshTokenToUser(ApplicationUser user, AuthDto authDto);
        Task ChangePasswordAsync(ChangePasswordDto passwordDto);
        Task<PatientDto> RegisterPatientAsync(PatientCreationDto creationDto);
        Task<AuthDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string token);
    }
}

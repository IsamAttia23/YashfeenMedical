using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.Infrastructure.UsersManagment
{
    public interface IUserManagmentServices
    {
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindUserByEmailAsync(string email);
        Task<ApplicationUser> FindUserAsync(string userId);
        Task<ApplicationUser> FindUserByNameAsync(string userName);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword ,string newPassword);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IdentityResult> AddUserToRole(ApplicationUser user,string role);
        Task<IEnumerable<Claim>> GetUserClaims(ApplicationUser user);
        Task<IList<string>> GetUserRoles(ApplicationUser user);
        Task<ApplicationUser> CheckRefreshTokenAsync(string refreshToken);
    }
}

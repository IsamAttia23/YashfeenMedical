using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.Infrastructure.JWT;

public interface IJwtService
{
    Task<JwtSecurityToken> GenerateAccessToken(ApplicationUser user);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

    DateTimeOffset GetAccessTokenExpiry();

    DateTimeOffset GetRefreshTokenExpiry();


    Task<ApplicationUser> GetUser(string id);

    Task<IEnumerable<Claim>> GetUserClaims(ApplicationUser user);
}

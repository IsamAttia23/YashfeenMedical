using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.Infrastructure.Exceptions;

namespace YashfeenMedical.Infrastructure.JWT;

public class JwtService : IJwtService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _settings;

    public JwtService(IOptions<JwtSettings> settings, UserManager<ApplicationUser> userManager)
    {
        _settings = settings.Value;
        _userManager = userManager;
    }

    public async Task<JwtSecurityToken> GenerateAccessToken(ApplicationUser user)
    {

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var signingCredrntals = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var claims = await GetUserClaims(user);

        var result = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpiryMinutes),
            signingCredentials: signingCredrntals
            );

        return result;
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _settings.Issuer,
            ValidateAudience = true,
            ValidAudience = _settings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
            ValidateLifetime = false // مهم: نسمح بقراءة توكن منتهي الصلاحية أثناء عملية الـRefresh فقط
        };

        var handler = new JwtSecurityTokenHandler();

        var principal = handler.ValidateToken(token, validationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return principal;
    }

    public DateTimeOffset GetAccessTokenExpiry() =>
        DateTimeOffset.UtcNow.AddMinutes(_settings.AccessTokenExpiryMinutes);

    public DateTimeOffset GetRefreshTokenExpiry() =>
        DateTimeOffset.UtcNow.AddDays(_settings.RefreshTokenExpiryDays);

    public async Task<IEnumerable<Claim>> GetUserClaims(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRolews = await _userManager.GetRolesAsync(user);
        var roleCalims = new List<Claim>();

        foreach (var role in userRolews)
            roleCalims.Add(new Claim("roles", role));

        var claims = new[]
        {
               new Claim(JwtRegisteredClaimNames.Sub , user.UserName),
               new Claim(JwtRegisteredClaimNames.Email , user.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim("userId",user.Id)
            }
        .Union(userClaims)
        .Union(roleCalims);

        return claims;
    }

    public async Task<ApplicationUser> GetUser(string email)
    {
        var result = await _userManager.FindByEmailAsync(email);

        if (result is null)
            throw new NotFoundException("the requested user not founded");
        return result;
    }
}

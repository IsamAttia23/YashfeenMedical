using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.Auth;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.IServices;

namespace YashfeenMedical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authServices.LoginAsync(loginDto);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authServices.RefreshTokenAsync(refreshToken);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogouAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            await _authServices.LogoutAsync(refreshToken);
            return Ok();
        }

        [HttpPost("register-patient")]
        public async Task<IActionResult> RegisterPatientAsync([FromForm] PatientCreationDto creationDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authServices.RegisterPatientAsync(creationDto);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto passwordDto)
        {
            await _authServices.ChangePasswordAsync(passwordDto);
            return Ok("password changed successfuly");
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTimeOffset expireDate)
        {
            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = expireDate.ToLocalTime(),
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOption);
        }
    }
}

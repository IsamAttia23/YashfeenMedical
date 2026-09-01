using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace YashfeenMedical.BLL.DTOs.Auth
{
    public class AuthDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpiration { get; set; }
    }
}

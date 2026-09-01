using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Users
{
    public class UserCreationDto
    {
        [Required(ErrorMessage = "user name is required")]
        [StringLength(50, ErrorMessage = "user name cannot exceed 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "password must be between 6 and 100 characters")]
        public string Password { get; set; }

        [Phone(ErrorMessage = "invalid phone number format")]
        public string? PhoneNumber { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YashfeenMedical.BLL.DTOs.Users
{
    public class UserUpdateModel
    {
        [Required(ErrorMessage = "user name is required")]
        [StringLength(50, ErrorMessage = "user name cannot exceed 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "invalid email format")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "invalid phone number format")]
        public string? PhoneNumber { get; set; }
    }
}

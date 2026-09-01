using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YashfeenMedical.BLL.DTOs.Users
{
    public class AddRoleToUserDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public IList<string> Roles { get; set; }
    }
}

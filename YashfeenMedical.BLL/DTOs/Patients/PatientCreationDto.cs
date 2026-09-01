using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YashfeenMedical.BLL.DTOs.Users;
using YashfeenMedical.DAL.Enums;

namespace YashfeenMedical.BLL.DTOs.Patients
{
    public class PatientCreationDto : UserCreationDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string NationalId { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

    
        public BloodType BloodType { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        [Required]
        public Gender Gender { get; set; }
    }
}

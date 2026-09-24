using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YashfeenMedical.BLL.DTOs.Users;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Patients
{
    public class PatientDto : UserDto, TIdType<int>
    {
        [Required]
        public int Id { get; set; }
        public string NationalId { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public BloodType BloodType { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public Gender Gender { get; set; }
    }
}

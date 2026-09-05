using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using YashfeenMedical.BLL.DTOs.Users;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Patients
{
    public class PatientUpdateDto : UserUpdateDto, TIdType<int>
    {
        [Required]
        public int Id { get; set; }

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
        public string? ProfilePhotoUrl { get; set; }
        public IFormFile? ProfilePhoto { get; set; }

        [Required]
        public Gender Gender { get; set; }
    }
}

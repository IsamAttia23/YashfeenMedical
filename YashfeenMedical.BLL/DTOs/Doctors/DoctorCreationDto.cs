using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using YashfeenMedical.BLL.DTOs.Users;

namespace YashfeenMedical.BLL.DTOs.Doctors
{
    public class DoctorCreationDto : UserCreationDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string LicenseNumber { get; set; }

        public string? Bio { get; set; }

        public bool IsAvailbe { get; set; } = true;

        [Required]
        [Range(0.1, double.MaxValue)]
        public decimal ConsultationFee { get; set; }

        public IFormFile? ProfilePhoto { get; set; }

        public IList<int>? SpecialtyIds { get; set; }
    }
}

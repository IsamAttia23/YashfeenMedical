using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using YashfeenMedical.BLL.DTOs.Users;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Doctors
{
    public class DoctorUpdateDto : UserUpdateDto, TIdType<int>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string LicenseNumber { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? Bio { get; set; }

        public bool IsAvailbe { get; set; }

        [Range(0.1, double.MaxValue)]
        public decimal ConsultationFee { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        public IFormFile? ProfilePhoto { get; set; }

        public IList<int>? SpecialtyIds { get; set; }
    }
}

using System;
using System.Collections.Generic;
using YashfeenMedical.BLL.DTOs.DoctorSchedules;
using YashfeenMedical.BLL.DTOs.Specialties;
using YashfeenMedical.BLL.DTOs.Users;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Doctors
{
    public class DoctorDto : UserDto, TIdType<int>
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string LicenseNumber { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? Bio { get; set; }

        public bool IsAvailbe { get; set; }

        public decimal ConsultationFee { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        public IList<string> Specialties { get; set; }
    }
}

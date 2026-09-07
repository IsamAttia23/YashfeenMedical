using System;
using System.Collections.Generic;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Doctors
{
    public class DoctorDto : TIdType<int>
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

        public IList<int>? ScheduleIds { get; set; }

        public IList<int>? SpecialtyIds { get; set; }
    }
}

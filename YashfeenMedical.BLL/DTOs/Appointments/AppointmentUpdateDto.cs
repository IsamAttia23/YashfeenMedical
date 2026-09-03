using System;
using System.ComponentModel.DataAnnotations;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Appointments
{
    public class AppointmentUpdateDto : TIdType<int>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateOnly AppointmentDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        public AppointmentStatus Status { get; set; }

        public AppointmentType Type { get; set; }

        [Required]
        public string ReasonForVisit { get; set; }

        public string? Notes { get; set; }
        public string? CancellationReason { get; set; }
    }
}

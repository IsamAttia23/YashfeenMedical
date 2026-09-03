using System;
using System.ComponentModel.DataAnnotations;
using YashfeenMedical.DAL.Enums;

namespace YashfeenMedical.BLL.DTOs.Appointments
{
    public class AppointmentCreationDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateOnly AppointmentDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        public AppointmentType Type { get; set; }

        [Required]
        public string ReasonForVisit { get; set; }

        public string? Notes { get; set; }
    }
}

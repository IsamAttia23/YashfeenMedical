using System.ComponentModel.DataAnnotations;

namespace YashfeenMedical.BLL.DTOs.DoctorSchedules
{
    public class DoctorScheduleCreationDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public int SlotDurationMinutes { get; set; }

        [Required]
        public int MaxAppointmentsPerDay { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

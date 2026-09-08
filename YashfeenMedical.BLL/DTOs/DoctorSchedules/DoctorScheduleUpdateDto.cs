using System.ComponentModel.DataAnnotations;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.DoctorSchedules
{
    public class DoctorScheduleUpdateDto : TIdType<int>
    {
        [Required]
        public int Id { get; set; }

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

        public bool IsActive { get; set; }
    }
}

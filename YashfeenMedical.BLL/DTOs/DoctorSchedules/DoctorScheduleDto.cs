using System;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.DoctorSchedules
{
    public class DoctorScheduleDto : TIdType<int>
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int SlotDurationMinutes { get; set; }

        public int MaxAppointmentsPerDay { get; set; }

        public bool IsActive { get; set; }
    }
}

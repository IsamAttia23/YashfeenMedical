using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.BLL.DTOs.DoctorSchedules
{
    public class AvailableSlotDto
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}

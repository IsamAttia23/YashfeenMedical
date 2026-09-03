using System;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Appointments
{
    public class AppointmentDto : TIdType<int>
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public AppointmentStatus Status { get; set; }

        public AppointmentType Type { get; set; }

        public string ReasonForVisit { get; set; }

        public string? Notes { get; set; }

        public string? CancellationReason { get; set; }

        public DateTimeOffset? ConfirmedAt { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateTimeOffset? CancelledAt { get; set; }
    }
}

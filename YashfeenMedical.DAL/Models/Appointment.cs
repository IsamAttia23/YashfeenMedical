using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YashfeenMedical.DAL.Models
{
    public class Appointment : TEntity<int>
    {
        [ForeignKey("PatientId")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [Required]
        public DateOnly AppointmentDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }

        [Required]
        public AppointmentType Type { get; set; }

        [Required]
        public string ReasonForVisit { get; set; }

        public string? Notes { get; set; }
        public string? CancellationReason { get; set; }

        public DateTimeOffset? ConfirmedAt { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateTimeOffset? CancelledAt { get; set; }

        public MedicalRecord MedicalRecord { get; set; }
        public Invoice Invoice { get; set; }
        public IList<MedicalFile> MedicalFiles { get; set; }
    }
}

using YashfeenMedical.DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YashfeenMedical.DAL.Models
{
    public class MedicalRecord : TEntity<int>
    {
        [ForeignKey("AppointmentId")]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        [Required]
        public string ChiefComplaint { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        public string? TreatmentPlan { get; set; }
        public string? BloodPressure { get; set; }
        public int? HeartRate { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string? DoctorNotes { get; set; }
        public DateOnly FollowUpDate { get; set; }

        public IList<Prescription> Prescriptions { get; set; }
    }
}

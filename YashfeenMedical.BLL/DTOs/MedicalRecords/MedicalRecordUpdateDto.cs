using System;
using System.ComponentModel.DataAnnotations;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.MedicalRecords
{
    public class MedicalRecordUpdateDto : TIdType<int>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

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

        public IList<int>? PrescriptionIds { get; set; }
    }
}

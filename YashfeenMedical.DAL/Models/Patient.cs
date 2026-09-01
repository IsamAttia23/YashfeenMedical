using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YashfeenMedical.DAL.Models
{
    public class Patient : TEntity<int>
    {
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string NationalId { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public BloodType BloodType { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? ProfilePhotoUrl { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public IList<Appointment> Appointments { get; set; }
        public IList<Invoice> Invoices { get; set; }
        public InsurancePolicy InsurancePolicy { get; set; }
        public IList<MedicalFile> MedicalFiles { get; set; }
    }
}

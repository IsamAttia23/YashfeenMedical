using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YashfeenMedical.BLL.DTOs.Prescriptions
{
    public class PrescriptionItemCreationDto
    {
        [Required]
        public int MedicationId { get; set; }
        [Required]
        public string Dosage { get; set; } = string.Empty;
        [Required]
        public string Frequency { get; set; } = string.Empty;
        [Required]
        public string Duration { get; set; } = string.Empty;
        public string? Instructions { get; set; }
    }

    public class PrescriptionCreationDto
    {
        [Required]
        public int MedicalRecordId { get; set; }

        public string? Notes { get; set; }

        public DateOnly? ExpiresAt { get; set; }

        public IList<PrescriptionItemCreationDto>? Items { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YashfeenMedical.DAL.Shared.Entities;
using YashfeenMedical.DAL.Enums;

namespace YashfeenMedical.BLL.DTOs.Prescriptions
{
    public class PrescriptionUpdateDto : TIdType<int>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int MedicalRecordId { get; set; }

        public string? Notes { get; set; }

        public DateOnly ExpiresAt { get; set; }

        public IList<PrescriptionItemDto>? Items { get; set; }

        // Status should not be set to Active by clients; keep status out of update DTO to enforce state machine.
    }
}

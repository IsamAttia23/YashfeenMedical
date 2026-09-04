using System;
using System.Collections.Generic;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Prescriptions
{
    public class PrescriptionDto : TIdType<int>
    {
        public int Id { get; set; }

        public int MedicalRecordId { get; set; }

        public string PrescriptionNumber { get; set; } = string.Empty;

        public PrescriptionStatus Status { get; set; }

        public string? Notes { get; set; }

        public DateOnly ExpiresAt { get; set; }

        public DateTimeOffset IssuedAt { get; set; }

        public IList<PrescriptionItemDto>? Items { get; set; }
    }
}

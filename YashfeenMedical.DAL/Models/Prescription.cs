using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YashfeenMedical.DAL.Models
{
    public class Prescription : TEntity<int>
    {
        [ForeignKey("MedicalRecordId")]
        public int MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        public string PrescriptionNumber { get; set; }
        public PrescriptionStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateOnly ExpiresAt { get; set; }
        public DateTimeOffset IssuedAt { get; set; }

        public IList<PrescriptionItem> Items { get; set; }
    }
}

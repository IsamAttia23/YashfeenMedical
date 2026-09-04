using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Prescriptions
{
    public class PrescriptionItemDto : TIdType<int>
    {
        public int Id { get; set; }

        public int PrescriptionId { get; set; }

        public int MedicationId { get; set; }

        public string Dosage { get; set; } = string.Empty;

        public string Frequency { get; set; } = string.Empty;

        public string Duration { get; set; } = string.Empty;

        public string? Instructions { get; set; }
    }
}

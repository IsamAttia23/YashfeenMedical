using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.DAL.Models;

// تفصيل كل دواء داخل الوصفة — يمثل علاقة Prescription (∞) → (∞) Medication
public class PrescriptionItem : TEntity<int>
{
    public int Id { get; set; }

    public int PrescriptionId { get; set; }
    public Prescription Prescription { get; set; } = null!;

    public int MedicationId { get; set; }
    public Medication Medication { get; set; } = null!;

    public string Dosage { get; set; } = string.Empty; 

    public string Frequency { get; set; } = string.Empty; 

    public string Duration { get; set; } = string.Empty; 

    public string? Instructions { get; set; } 
}

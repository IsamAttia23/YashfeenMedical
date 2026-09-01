using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.DAL.Models;

public class Medication : TEntity<int>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty; 

    public string GenericName { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty; 

    public string Unit { get; set; } = string.Empty; 

    public bool RequiresPrescription { get; set; }

    public bool IsAvailable { get; set; } = true;

    
    public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
}

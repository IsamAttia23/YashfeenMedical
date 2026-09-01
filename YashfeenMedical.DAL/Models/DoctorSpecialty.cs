namespace YashfeenMedical.DAL.Models;

// جدول الربط لعلاقة Many-to-Many بين Doctor و Specialty
public class DoctorSpecialty
{
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public int SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = null!;
}

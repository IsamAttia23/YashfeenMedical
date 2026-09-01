using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.DAL.Models;

public class MedicalFile : TEntity<int>
{
    public int Id { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public int? AppointmentId { get; set; } // اختياري
    public Appointment? Appointment { get; set; }

    public string UploadedByUserId { get; set; }
    public ApplicationUser UploadedByUser { get; set; } = null!;

    public FileType FileType { get; set; }

    public string FileName { get; set; } = string.Empty; // الاسم الأصلي

    public string StoredFileName { get; set; } = string.Empty; // GUID + extension

    public string FileUrl { get; set; } = string.Empty;

    public long FileSizeKB { get; set; }

    public string MimeType { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
}

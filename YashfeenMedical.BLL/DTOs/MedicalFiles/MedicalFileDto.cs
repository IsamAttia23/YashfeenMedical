using System;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.MedicalFiles
{
    public class MedicalFileDto : TIdType<int>
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int? AppointmentId { get; set; }

        public string UploadedByUserId { get; set; } = string.Empty;

        public FileType FileType { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty;

        public long FileSizeKB { get; set; }

        public string MimeType { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTimeOffset UploadedAt { get; set; }
    }
}

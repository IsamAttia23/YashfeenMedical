using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using YashfeenMedical.DAL.Enums;

namespace YashfeenMedical.BLL.DTOs.MedicalFiles
{
    public class MedicalFileCreationDto
    {
        [Required]
        public int PatientId { get; set; }

        public int? AppointmentId { get; set; }

        [Required]
        public string UploadedByUserId { get; set; }

        [Required]
        public FileType FileType { get; set; }

        [Required]
        public IFormFile File { get; set; }

        public string? Description { get; set; }
    }
}

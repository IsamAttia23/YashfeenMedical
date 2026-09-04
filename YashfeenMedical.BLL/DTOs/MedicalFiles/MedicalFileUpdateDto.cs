using System.ComponentModel.DataAnnotations;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.MedicalFiles
{
    public class MedicalFileUpdateDto : TIdType<int>
    {
        [Required]
        public int Id { get; set; }

        public string? Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.DTOs.Specialties
{
    public class SpecialtyUpdateDto : TIdType<int>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}

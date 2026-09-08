using System.ComponentModel.DataAnnotations;

namespace YashfeenMedical.BLL.DTOs.Specialties
{
    public class SpecialtyCreationDto
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}

using YashfeenMedical.DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YashfeenMedical.DAL.Models
{
    public class Specialty : TEntity<int>
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        public IList<DoctorSpecialty> DoctorSpecialties { get; set; }

    }
}

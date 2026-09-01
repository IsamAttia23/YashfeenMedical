using YashfeenMedical.DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YashfeenMedical.DAL.Models
{
    public class Doctor : TEntity<int>
    {

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string LicenseNumber { get; set; }

        [Required]
        public string Phone { get; set; }
        public string? Bio { get; set; }
        public bool IsAvailbe { get; set; }

        [Required, Range(0.1, double.MaxValue)]
        public decimal ConsultationFee { get; set; }
        public string? ProfilePhotoUrl { get; set; }

        public IList<DoctorSchedule> Schedules { get; set; }
        public IList<Appointment> Appointments { get; set; }
        public IList<DoctorSpecialty> DoctorSpecialties { get; set; }

    }
}

using YashfeenMedical.DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using YashfeenMedical.DAL.IRepositories;

namespace YashfeenMedical.DAL.Models
{
    public class Doctor : TEntity<int> , IRowVersionProperty
    {

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string LicenseNumber { get; set; }
        public string? Bio { get; set; }
        public bool IsAvailable { get; set; }

        [Required, Range(0.1, double.MaxValue)]
        public decimal ConsultationFee { get; set; }
        public string? ProfilePhotoUrl { get; set; }

        public IList<DoctorSchedule> Schedules { get; set; }
        public IList<Appointment> Appointments { get; set; }
        public IList<DoctorSpecialty> DoctorSpecialties { get; set; }
        public IList<Specialty> Specialties { get; set; }

        public byte[] RowVersion { get; set; }

    }
}

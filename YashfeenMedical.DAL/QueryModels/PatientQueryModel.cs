using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.DAL.Enums;

namespace YashfeenMedical.DAL.QueryModels
{
    public class PatientQueryModel
    {
        public string? Search { get; set; }

        public string? FullName { get; set; }

        public string? NationalId { get; set; }

        public BloodType? BloodType { get; set; }

        public Gender? Gender { get; set; }

        public DateOnly? DateOfBirthFrom { get; set; }

        public DateOnly? DateOfBirthTo { get; set; }

        public string? Address { get; set; }

        public bool? HasAllergies { get; set; }

        public bool? HasChronicDiseases { get; set; }

        //sorting
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
}

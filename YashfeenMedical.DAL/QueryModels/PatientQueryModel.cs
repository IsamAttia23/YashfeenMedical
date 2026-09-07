using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.DAL.Enums;

namespace YashfeenMedical.DAL.QueryModels
{
    public class PatientQueryModel : PaginationQuery
    {
        public string? FullName { get; set; }

        public string? NationalId { get; set; }

        public string? Phone { get; set; }

        public bool? IsActive { get; set; }

        public BloodType? BloodType { get; set; }

        public Gender? Gender { get; set; }

        public int? AgeFrom { get; set; }

        public int? AgeTo { get; set; }
    }
}

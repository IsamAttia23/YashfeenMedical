using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.QueryModels
{
    public class DoctorQueryModel : PaginationQuery
    {
        public int? SpecialtyId { get; set; }
    }
}

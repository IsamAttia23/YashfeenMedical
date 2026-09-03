using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.DAL.Enums;

namespace YashfeenMedical.DAL.QueryModels
{
    public class PatientAppointmentsQueryModel : PaginationQuery
    {
        public AppointmentStatus? Status { get; set; }

        public DateOnly? AgeFrom { get; set; }

        public DateOnly? AgeTo { get; set; }

        public int? DoctorId { get; set; }

        public AppointmentType? Type { get; set; }
    }
}

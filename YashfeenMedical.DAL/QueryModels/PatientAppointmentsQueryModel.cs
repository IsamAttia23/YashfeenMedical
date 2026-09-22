using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.DAL.Enums;

namespace YashfeenMedical.DAL.QueryModels
{
    public class PatientAppointmentsQueryModel : AppointmentQueryModel
    {
        public PatientAppointmentsQueryModel()
        {
            SortDirection = Enums.SortDirection.Descending;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;

namespace YashfeenMedical.BLL.DTOs.Patients
{
    public class PaitentAppointmentsDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public IList<AppointmentDto> Appointments { get; set; }
    }
}

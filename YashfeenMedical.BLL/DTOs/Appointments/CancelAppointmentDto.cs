using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YashfeenMedical.BLL.DTOs.Appointments
{
    public class CancelAppointmentDto
    {
        [Required]
        public string CancellationReason { get; set; } = string.Empty;
    }
}

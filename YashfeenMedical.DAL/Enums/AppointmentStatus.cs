using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.Enums
{
    public enum AppointmentStatus
    {
        Scheduled,
        Confirmed,
        InProgress,
        Pending,
        Completed,
        Cancelled,
        NoShow
    }
}

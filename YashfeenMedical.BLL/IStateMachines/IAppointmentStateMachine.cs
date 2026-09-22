using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;

namespace YashfeenMedical.BLL.IStateMachines
{
    public interface IAppointmentStateMachine
    {
        Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId);
        Task<AppointmentDto> StartAppointmentAsync(int appointmentId);
        Task<AppointmentDto> CancelAppointmentAsync(int appointmentId,string cancelReason);
        Task<AppointmentDto> AppointmentSetAppointmentAsNoShowAsync(int appointmentId);
    }
}

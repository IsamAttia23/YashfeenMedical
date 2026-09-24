using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.MedicalRecords;

namespace YashfeenMedical.BLL.IStateMachines
{
    public interface IAppointmentStateMachine
    {
        Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId);
        Task<AppointmentDto> StartAppointmentAsync(int appointmentId);
        Task<AppointmentDto> CancelAppointmentAsync(int appointmentId,string cancelReason);
        Task<AppointmentDto> SetAppointmentAsNoShowAsync(int appointmentId);
        Task<(AppointmentDto, MedicalRecordDto)> CompleteAppointmentAsync(int appointmentId, MedicalRecordCreationDto medicalRecord);
    }
}

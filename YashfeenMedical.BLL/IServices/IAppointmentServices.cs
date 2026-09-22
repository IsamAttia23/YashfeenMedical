using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.BLL.IServices
{
    public interface IAppointmentServices : IEntityServices<int, AppointmentDto, AppointmentCreationDto, AppointmentUpdateDto>
    {
        Task<TPaginationQueryModel<AppointmentDto>> GetFilterdAppointmentsAsync(AppointmentQueryModel queryModel);

        Task<TPaginationQueryModel<AppointmentDto>> BringAppointmentsTodayAsync(AppointmentQueryModel queryModel, DateOnly date);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.Doctors;
using YashfeenMedical.BLL.DTOs.DoctorSchedules;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.BLL.IServices
{
    public interface IDoctorServices : IEntityServices<int, DoctorDto, DoctorCreationDto, DoctorUpdateDto>
    {
        Task<TPaginationQueryModel<DoctorDto>> GetFilteredDoctorsWithPaginationAsync(DoctorQueryModel queryModel);

        Task<TPaginationQueryModel<DoctorScheduleDto>> GetDoctorSchedule(int doctorId, PaginationQuery paginationQuery);
        Task<TPaginationQueryModel<AppointmentDto>> GetDoctorAppointments(int doctorId, PaginationQuery paginationQuery);
        Task<DoctorScheduleDto> UpsertSchedule(int doctorId, DoctorScheduleUpdateDto doctorSchedule);

        Task<List<AvailableSlotDto>> GetDoctorScheduleOnDayAsync(int doctorId, DateOnly date);
    }
}

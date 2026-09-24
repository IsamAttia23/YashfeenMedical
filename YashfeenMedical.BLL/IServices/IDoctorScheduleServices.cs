using YashfeenMedical.BLL.DTOs.DoctorSchedules;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.BLL.IServices
{
    public interface IDoctorScheduleServices : IEntityServices<int, DoctorScheduleDto, DoctorScheduleCreationDto, DoctorScheduleUpdateDto>
    {
        Task<TPaginationQueryModel<DoctorScheduleDto>> GetDoctorSchedule(int doctorId, PaginationQuery paginationQuery);
    }
}

using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IDoctorScheduleRepository : IRepository<DoctorSchedule, int>
    {
        IQueryable<DoctorSchedule> GetDoctorScheduleAsync(int doctorId);
        Task<DoctorSchedule> GetByDoctorAndDayAsync(int doctorId, Enums.ScheduleDayOfWeek dayOfWeek);
    }
}

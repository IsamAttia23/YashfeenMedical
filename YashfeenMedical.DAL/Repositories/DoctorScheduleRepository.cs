using System.Linq;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace YashfeenMedical.DAL.Repositories
{
    public class DoctorScheduleRepository : TRepository<DoctorSchedule, int>, IDoctorScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<DoctorSchedule> SelectQuery => _context.Set<DoctorSchedule>()
            .Where(ds=> ds.DeletedOn == null);

        public DoctorScheduleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<DoctorSchedule> GetDoctorScheduleAsync(int doctorId)
        {
            var schedules = SelectQuery.Where(ds => ds.DoctorId == doctorId);
            return schedules;
        }

        public async Task<DoctorSchedule> GetByDoctorAndDayAsync(int doctorId, ScheduleDayOfWeek dayOfWeek)
        {
            var schedule = await SelectQuery.FirstOrDefaultAsync(ds => ds.DoctorId == doctorId && ds.DayOfWeek == dayOfWeek);
            return schedule;
        }
    }
}

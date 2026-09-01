using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class DoctorScheduleRepository : TRepository<DoctorSchedule, int>, IDoctorScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<DoctorSchedule> SelectQuery => _context.Set<DoctorSchedule>();

        public DoctorScheduleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

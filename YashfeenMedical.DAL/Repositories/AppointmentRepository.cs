using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class AppointmentRepository : TRepository<Appointment, int>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Appointment> SelectQuery => _context.Set<Appointment>();

        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

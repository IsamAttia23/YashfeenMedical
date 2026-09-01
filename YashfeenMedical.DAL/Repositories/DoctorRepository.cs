using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class DoctorRepository : TRepository<Doctor, int>, IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Doctor> SelectQuery => _context.Set<Doctor>();

        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

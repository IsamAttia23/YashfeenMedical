using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class PrescriptionRepository : TRepository<Prescription, int>, IPrescriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Prescription> SelectQuery => _context.Set<Prescription>();

        public PrescriptionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

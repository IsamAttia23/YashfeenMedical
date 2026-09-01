using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class MedicationRepository : TRepository<Medication, int>, IMedicationRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Medication> SelectQuery => _context.Set<Medication>();

        public MedicationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

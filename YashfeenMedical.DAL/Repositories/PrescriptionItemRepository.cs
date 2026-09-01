using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class PrescriptionItemRepository : TRepository<PrescriptionItem, int>, IPrescriptionItemRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<PrescriptionItem> SelectQuery => _context.Set<PrescriptionItem>();

        public PrescriptionItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

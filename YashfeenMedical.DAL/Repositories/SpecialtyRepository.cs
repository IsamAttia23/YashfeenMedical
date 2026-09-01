using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class SpecialtyRepository : TRepository<Specialty, int>, ISpecialtyRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Specialty> SelectQuery => _context.Set<Specialty>();

        public SpecialtyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

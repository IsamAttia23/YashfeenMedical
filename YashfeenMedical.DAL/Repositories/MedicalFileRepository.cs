using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class MedicalFileRepository : TRepository<MedicalFile, int>, IMedicalFileRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<MedicalFile> SelectQuery => _context.Set<MedicalFile>();

        public MedicalFileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

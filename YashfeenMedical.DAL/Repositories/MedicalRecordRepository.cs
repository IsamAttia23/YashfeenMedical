using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class MedicalRecordRepository : TRepository<MedicalRecord, int>, IMedicalRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<MedicalRecord> SelectQuery => _context.Set<MedicalRecord>();

        public MedicalRecordRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class MedicalFileRepository : TRepository<MedicalFile, int>, IMedicalFileRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<MedicalFile> SelectQuery => _context.Set<MedicalFile>()
            .Where(m => m.DeletedOn == null);

        public MedicalFileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IQueryable<MedicalFile>> GetMedicalFileByPatientId(int patientId)
        {
            var result = _context.MedicalFiles.Where(m => m.PatientId == patientId);
            return result;
        }
    }
}

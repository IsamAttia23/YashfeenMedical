using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class PrescriptionRepository : TRepository<Prescription, int>, IPrescriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Prescription> SelectQuery => _context.Set<Prescription>()
            .Where(p => p.DeletedOn == null);

        public PrescriptionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IQueryable<Prescription>> GetPrescriptionsByPatientId(int patientId)
        {
            var result = SelectQuery.Where(p => p.MedicalRecord.Appointment.PatientId == patientId);

            return result; 
        }
    }
}

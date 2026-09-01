using Microsoft.EntityFrameworkCore;
using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class PatientRepository : TRepository<Patient, int>, IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Patient> SelectQuery => _context.Set<Patient>()
            .Where(p=> p.DeletedOn == null).Include(p=> p.ApplicationUser);

        public PatientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

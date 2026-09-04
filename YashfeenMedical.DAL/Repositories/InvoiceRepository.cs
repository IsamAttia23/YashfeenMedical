using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace YashfeenMedical.DAL.Repositories
{
    public class InvoiceRepository : TRepository<Invoice, int>, IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Invoice> SelectQuery => _context.Set<Invoice>()
            .Where(i => i.DeletedOn == null);

        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IQueryable<Invoice>> GetInvoicesByPatientId(int patientId)
        {
            var result = SelectQuery.Where(i => i.PatientId == patientId);
            return result;
        }
    }
}

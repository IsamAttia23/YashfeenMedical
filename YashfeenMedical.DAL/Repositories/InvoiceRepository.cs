using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace YashfeenMedical.DAL.Repositories
{
    public class InvoiceRepository : TRepository<Invoice, int>, IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Invoice> SelectQuery => _context.Set<Invoice>();

        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

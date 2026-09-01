using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class InvoiceItemRepository : TRepository<InvoiceItem, int>, IInvoiceItemRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<InvoiceItem> SelectQuery => _context.Set<InvoiceItem>();

        public InvoiceItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

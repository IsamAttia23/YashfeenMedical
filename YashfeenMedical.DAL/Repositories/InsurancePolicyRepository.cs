using System.Linq;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.Repositories
{
    public class InsurancePolicyRepository : TRepository<InsurancePolicy, int>, IInsurancePolicyRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<InsurancePolicy> SelectQuery => _context.Set<InsurancePolicy>();

        public InsurancePolicyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

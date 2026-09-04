using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IInvoiceRepository : IRepository<Invoice, int>
    {
        Task<IQueryable<Invoice>> GetInvoicesByPatientId(int patientId);
    }
}

using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IPrescriptionRepository : IRepository<Prescription, int>
    {
        Task<IQueryable<Prescription>> GetPrescriptionsByPatientId(int patientId);
    }
}

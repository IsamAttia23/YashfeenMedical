using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IMedicalRecordRepository : IRepository<MedicalRecord, int>
    {
        Task<IQueryable<MedicalRecord>> GetByPatientId(int patientId);
    }
}

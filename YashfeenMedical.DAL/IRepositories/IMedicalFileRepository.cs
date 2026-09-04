using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IMedicalFileRepository : IRepository<MedicalFile, int>
    {
        Task<IQueryable<MedicalFile>> GetMedicalFileByPatientId(int patientId);
    }
}

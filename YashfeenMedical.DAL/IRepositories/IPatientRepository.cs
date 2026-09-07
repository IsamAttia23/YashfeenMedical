using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IPatientRepository : IRepository<Patient, int>
    {
        Task<IQueryable<Patient>> GetFilteredPatientsAsync(PatientQueryModel queryModel);
    }
}

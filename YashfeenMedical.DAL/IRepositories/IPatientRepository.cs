using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IPatientRepository : IRepository<Patient, int>
    {
        IQueryable<Patient> GetFilteredPatientsAsync(PatientQueryModel queryModel);
        Task<TPaginationQueryModel<Patient>> GetFilteredPatientsWithPaginationAsync(PatientQueryModel queryModel,PaginationQuery paginationQuery);
    }
}

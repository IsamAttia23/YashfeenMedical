using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IDoctorRepository : IRepository<Doctor, int>
    {
        Task<IQueryable<Doctor>> GetFilteredDoctorsAsync(DoctorQueryModel queryModel);
        Task<TPaginationQueryModel<Doctor>> GetFilteredDoctorsWithPaginationAsync(DoctorQueryModel queryModel);
    }
}

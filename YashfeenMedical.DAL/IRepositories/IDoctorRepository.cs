using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IDoctorRepository : IRepository<Doctor, int>
    {
        IQueryable<Doctor> GetFilteredDoctorsAsync(DoctorQueryModel queryModel);
    }
}

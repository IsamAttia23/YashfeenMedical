using Microsoft.EntityFrameworkCore;
using System.Linq;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.DAL.Repositories
{
    public class DoctorRepository : TRepository<Doctor, int>, IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Doctor> SelectQuery => _context.Set<Doctor>()
            .Where(d => d.DeletedOn == null)
            .Include(ds => ds.DoctorSpecialties)
            .Include(s => s.Schedules);

        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IQueryable<Doctor>> GetFilteredDoctorsAsync(DoctorQueryModel queryModel)
        {
            var doctors = SelectQuery;

            if (queryModel.SpecialtyId.HasValue)
            {
                doctors = doctors.Where(d =>
                    d.DoctorSpecialties.Any(ds =>
                        ds.SpecialtyId == queryModel.SpecialtyId.Value));
            }

            if (!string.IsNullOrWhiteSpace(queryModel.SearchTerm))
            {
                doctors = doctors.Where(d =>
                    d.FullName.Contains(queryModel.SearchTerm) ||
                    d.ApplicationUser.Email!.Contains(queryModel.SearchTerm));
            }

            doctors = queryModel.SortBy?.ToLower() switch
            {
                "fullname" => queryModel.SortDirection == SortDirection.Descending
                    ? doctors.OrderByDescending(d => d.FullName)
                    : doctors.OrderBy(d => d.FullName),
                "specialty" => queryModel.SortDirection == SortDirection.Descending
                    ? doctors.OrderByDescending(p => p.DoctorSpecialties)
                    : doctors.OrderBy(p => p.DoctorSpecialties),

                _ => doctors.OrderBy(d => d.FullName)
            };

            return doctors;
        }

        public Task<TPaginationQueryModel<Doctor>> GetFilteredDoctorsWithPaginationAsync(DoctorQueryModel queryModel)
        {
            throw new NotImplementedException();
        }
    }
}

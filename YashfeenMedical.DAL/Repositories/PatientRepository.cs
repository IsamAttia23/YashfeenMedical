using Microsoft.EntityFrameworkCore;
using System.Linq;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace YashfeenMedical.DAL.Repositories
{
    public class PatientRepository : TRepository<Patient, int>, IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Patient> SelectQuery => _context.Set<Patient>()
            .Where(p => p.DeletedOn == null)
            .Include(p => p.ApplicationUser);

        public PatientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IQueryable<Patient>> GetFilteredPatientsAsync(PatientQueryModel queryModel)
        {
            var patients = SelectQuery;

            if (!string.IsNullOrWhiteSpace(queryModel.SearchTerm))
            {
                patients = patients.Where(p =>
                    p.FullName.Contains(queryModel.SearchTerm) ||
                    p.NationalId.Contains(queryModel.SearchTerm) ||
                    p.ApplicationUser.Email!.Contains(queryModel.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(queryModel.FullName))
            {
                patients = patients.Where(p =>
                    p.FullName.Contains(queryModel.FullName));
            }

            if (!string.IsNullOrWhiteSpace(queryModel.NationalId))
            {
                patients = patients.Where(p =>
                    p.NationalId.Contains(queryModel.NationalId));
            }

            if (queryModel.IsActive.HasValue)
            {
                patients = queryModel.IsActive == true
                ? patients.Where(p => p.ApplicationUser.IsActive == true)
                : patients.Where(p => p.ApplicationUser.IsActive == false);
            }

            if (queryModel.BloodType.HasValue)
            {
                patients = patients.Where(p =>
                    p.BloodType == queryModel.BloodType.Value);
            }

            if (queryModel.Gender.HasValue)
            {
                patients = patients.Where(p =>
                    p.Gender == queryModel.Gender.Value);
            }

            if (queryModel.AgeFrom.HasValue)
            {
                var maxBirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-queryModel.AgeFrom.Value));

                patients = patients.Where(p =>
                p.DateOfBirth >= maxBirthDate);
            }

            if (queryModel.AgeTo.HasValue)
            {
                var minBirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-(queryModel.AgeFrom.Value+1)));

                patients = patients.Where(p =>
                p.DateOfBirth <= minBirthDate);
            }

            // Sorting
            patients = queryModel.SortBy?.ToLower() switch
            {
                "fullname" => queryModel.SortDirection == SortDirection.Descending
                    ? patients.OrderByDescending(p => p.FullName)
                    : patients.OrderBy(p => p.FullName),

                "dateofbirth" => queryModel.SortDirection == SortDirection.Descending
                    ? patients.OrderByDescending(p => p.DateOfBirth)
                    : patients.OrderBy(p => p.DateOfBirth),

                "nationalid" => queryModel.SortDirection == SortDirection.Descending
                    ? patients.OrderByDescending(p => p.NationalId)
                    : patients.OrderBy(p => p.NationalId),

                _ => patients.OrderBy(p => p.FullName)
            };
            return patients;
        }

        public async Task<TPaginationQueryModel<Patient>> GetFilteredPatientsWithPaginationAsync(PatientQueryModel queryModel)
        {
            var patients = await GetFilteredPatientsAsync(queryModel);

            var paggedOrders = await GetPaggedList(patients, queryModel);

            return paggedOrders;
        }

    }
}

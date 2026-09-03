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

        public IQueryable<Patient> GetFilteredPatientsAsync(PatientQueryModel queryModel)
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
                patients = patients.Where(p =>
                    p.DateOfBirth >= queryModel.AgeFrom.Value);
            }

            if (queryModel.AgeTo.HasValue)
            {
                patients = patients.Where(p =>
                    p.DateOfBirth <= queryModel.AgeTo.Value);
            }

            // Sorting
            patients = queryModel.SortBy?.ToLower() switch
            {
                "fullname" => queryModel.SortDirection == SortDirection.Ascending
                    ? patients.OrderByDescending(p => p.FullName)
                    : patients.OrderBy(p => p.FullName),

                "dateofbirth" => queryModel.SortDirection == SortDirection.Ascending
                    ? patients.OrderByDescending(p => p.DateOfBirth)
                    : patients.OrderBy(p => p.DateOfBirth),

                "nationalid" => queryModel.SortDirection == SortDirection.Ascending
                    ? patients.OrderByDescending(p => p.NationalId)
                    : patients.OrderBy(p => p.NationalId),

                _ => patients.OrderBy(p => p.FullName)
            };
            return patients;
        }

        public async Task<TPaginationQueryModel<Patient>> GetFilteredPatientsWithPaginationAsync(PatientQueryModel queryModel)
        {
            var ordersList = GetFilteredPatientsAsync(queryModel)
               .Skip((queryModel.PageNumber - 1) * queryModel.PageSize)
               .Take(queryModel.PageSize);

            var paggedOrders = GetPaggedList(ordersList, queryModel);



            return await paggedOrders;
        }

    }
}

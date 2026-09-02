using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            .Where(p => p.DeletedOn == null).Include(p => p.ApplicationUser);

        public PatientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Patient> GetFilteredPatientsAsync(PatientQueryModel queryModel)
        {
            var patients = SelectQuery;

            if (!string.IsNullOrWhiteSpace(queryModel.Search))
            {
                patients = patients.Where(p =>
                    p.FullName.Contains(queryModel.Search) ||
                    p.NationalId.Contains(queryModel.Search) ||
                    p.ApplicationUser.Email!.Contains(queryModel.Search));
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

            if (queryModel.DateOfBirthFrom.HasValue)
            {
                patients = patients.Where(p =>
                    p.DateOfBirth >= queryModel.DateOfBirthFrom.Value);
            }

            if (queryModel.DateOfBirthTo.HasValue)
            {
                patients = patients.Where(p =>
                    p.DateOfBirth <= queryModel.DateOfBirthTo.Value);
            }

            if (queryModel.HasAllergies.HasValue)
            {
                if (queryModel.HasAllergies.Value)
                {
                    patients = patients.Where(p =>
                        p.Allergies != null &&
                        p.Allergies != "");
                }
                else
                {
                    patients = patients.Where(p =>
                        p.Allergies == null ||
                        p.Allergies == "");
                }
            }

            if (queryModel.HasChronicDiseases.HasValue)
            {
                if (queryModel.HasChronicDiseases.Value)
                {
                    patients = patients.Where(p =>
                        p.ChronicDiseases != null &&
                        p.ChronicDiseases != "");
                }
                else
                {
                    patients = patients.Where(p =>
                        p.ChronicDiseases == null ||
                        p.ChronicDiseases == "");
                }
            }

            // Sorting
            patients = queryModel.SortBy?.ToLower() switch
            {
                "fullname" => queryModel.SortDescending
                    ? patients.OrderByDescending(p => p.FullName)
                    : patients.OrderBy(p => p.FullName),

                "dateofbirth" => queryModel.SortDescending
                    ? patients.OrderByDescending(p => p.DateOfBirth)
                    : patients.OrderBy(p => p.DateOfBirth),

                "nationalid" => queryModel.SortDescending
                    ? patients.OrderByDescending(p => p.NationalId)
                    : patients.OrderBy(p => p.NationalId),

                _ => patients.OrderBy(p => p.FullName)
            };
            return patients;
        }

        public async Task<TPaginationQueryModel<Patient>> GetFilteredPatientsWithPaginationAsync(PatientQueryModel queryModel, PaginationQuery paginationQuery)
        {
            var ordersList = GetFilteredPatientsAsync(queryModel)
               .Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
               .Take(paginationQuery.PageSize);

            var paggedOrders = GetPaggedList(ordersList, paginationQuery);



            return await paggedOrders;
        }
    }
}

using System.Linq;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace YashfeenMedical.DAL.Repositories
{
    public class AppointmentRepository : TRepository<Appointment, int>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public override IQueryable<Appointment> SelectQuery => _context.Set<Appointment>().Where(a => a.DeletedOn == null);

        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IQueryable<Appointment>> GetPatientAppointmentsAsync(int patientId)
        {
            var result = SelectQuery.Where(a => a.PatientId == patientId);
            return result;
        }

        public async Task<IQueryable<Appointment>> GetFilterdAppointmentsAsync(PatientAppointmentsQueryModel queryModel,
            IQueryable<Appointment> patientAppointments)
        {
            if (queryModel.Status.HasValue)
            {
                patientAppointments = patientAppointments.Where(a =>
                    a.Status == queryModel.Status.Value);
            }

            // Doctor
            if (queryModel.DoctorId.HasValue)
            {
                patientAppointments = patientAppointments.Where(a =>
                    a.DoctorId == queryModel.DoctorId.Value);
            }

            // Appointment Type
            if (queryModel.Type.HasValue)
            {
                patientAppointments = patientAppointments.Where(a =>
                    a.Type == queryModel.Type.Value);
            }

            // Patient Date Of Birth From
            if (queryModel.AgeFrom.HasValue)
            {
                patientAppointments = patientAppointments.Where(a =>
                    a.Patient.DateOfBirth >= queryModel.AgeFrom.Value);
            }

            // Patient Date Of Birth To
            if (queryModel.AgeTo.HasValue)
            {
                patientAppointments = patientAppointments.Where(a =>
                    a.Patient.DateOfBirth <= queryModel.AgeTo.Value);
            }

            // Search
            if (!string.IsNullOrWhiteSpace(queryModel.SearchTerm))
            {
                var search = queryModel.SearchTerm.Trim();

                patientAppointments = patientAppointments.Where(a =>
                    a.Doctor.FullName.Contains(search) ||
                    a.Patient.FullName.Contains(search));
            }

            // Sorting
            var sortBy = queryModel.SortBy?.ToLower();

            patientAppointments = sortBy switch
            {
                "date" => queryModel.SortDirection == SortDirection.Descending
                    ? patientAppointments.OrderByDescending(a => a.AppointmentDate)
                    : patientAppointments.OrderBy(a => a.AppointmentDate),

                "status" => queryModel.SortDirection == SortDirection.Descending
                    ? patientAppointments.OrderByDescending(a => a.Status)
                    : patientAppointments.OrderBy(a => a.Status),

                "type" => queryModel.SortDirection == SortDirection.Descending
                    ? patientAppointments.OrderByDescending(a => a.Type)
                    : patientAppointments.OrderBy(a => a.Type),

                _ => patientAppointments.OrderBy(a => a.AppointmentDate)
            };

            return patientAppointments;
        }
    }
}

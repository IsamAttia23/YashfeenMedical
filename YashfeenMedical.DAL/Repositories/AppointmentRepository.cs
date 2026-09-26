using Microsoft.EntityFrameworkCore;
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

        public override IQueryable<Appointment> SelectQuery => _context.Set<Appointment>().Where(a => a.DeletedOn == null)
            .Include(a => a.Invoice);

        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Appointment> GetPatientAppointments(int patientId)
        {
            var result = SelectQuery.Where(a => a.PatientId == patientId);
            return result;
        }

        public IQueryable<Appointment> GetFilterdAppointments(AppointmentQueryModel queryModel, IQueryable<Appointment> appointments)
        {
            if (queryModel.Status.HasValue)
            {
                appointments = appointments.Where(a =>
                    a.Status == queryModel.Status.Value);
            }

            //Patient
            if (queryModel.PatientId.HasValue)
            {
                appointments = appointments.Where(a =>
                   a.PatientId == queryModel.PatientId.Value);
            }

            // Doctor
            if (queryModel.DoctorId.HasValue)
            {
                appointments = appointments.Where(a =>
                    a.DoctorId == queryModel.DoctorId.Value);
            }

            // Appointment Type
            if (queryModel.Type.HasValue)
            {
                appointments = appointments.Where(a =>
                    a.Type == queryModel.Type.Value);
            }

            if (queryModel.DateFrom.HasValue)
            {
                appointments = appointments.Where(a =>
                    a.AppointmentDate <= queryModel.DateFrom.Value);
            }

            // Search
            if (!string.IsNullOrWhiteSpace(queryModel.SearchTerm))
            {
                var search = queryModel.SearchTerm.Trim();

                appointments = appointments.Where(a =>
                    a.Doctor.FullName.Contains(search) ||
                    a.Patient.FullName.Contains(search));
            }

            // Sorting
            var sortBy = queryModel.SortBy?.ToLower();

            appointments = sortBy switch
            {
                "date" => queryModel.SortDirection == SortDirection.Descending
                    ? appointments.OrderByDescending(a => a.AppointmentDate)
                    : appointments.OrderBy(a => a.AppointmentDate),

                "status" => queryModel.SortDirection == SortDirection.Descending
                    ? appointments.OrderByDescending(a => a.Status)
                    : appointments.OrderBy(a => a.Status),

                "type" => queryModel.SortDirection == SortDirection.Descending
                    ? appointments.OrderByDescending(a => a.Type)
                    : appointments.OrderBy(a => a.Type),

                _ => appointments.OrderBy(a => a.AppointmentDate)
            };

            return appointments;
        }

        public IQueryable<Appointment> GetDoctorAppointments(int doctorId)
        {
            var result = SelectQuery.Where(a => a.DoctorId == doctorId);
            return result;
        }

        public IQueryable<Appointment> BringAppointmentsToday(DateOnly date)
        {
            var result = SelectQuery.Where(a => a.AppointmentDate == date);
            return result;
        }
    }
}

using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IAppointmentRepository : IRepository<Appointment, int>
    {
        IQueryable<Appointment> GetPatientAppointments(int patientId);
        IQueryable<Appointment> GetFilterdAppointments(AppointmentQueryModel queryModel, IQueryable<Appointment> patientAppointments);
        IQueryable<Appointment> GetDoctorAppointments(int doctorId);
        IQueryable<Appointment> BringAppointmentsToday(DateOnly date);
    }
}

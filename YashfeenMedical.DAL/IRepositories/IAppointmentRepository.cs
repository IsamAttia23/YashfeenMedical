using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IAppointmentRepository : IRepository<Appointment, int>
    {
        IQueryable<Appointment> GetPatientAppointmentsAsync(int patientId);
        IQueryable<Appointment> GetFilterdAppointmentsAsync(PatientAppointmentsQueryModel queryModel, IQueryable<Appointment> patientAppointments);
    }
}

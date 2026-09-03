using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IAppointmentRepository : IRepository<Appointment, int>
    {
        Task<IQueryable<Appointment>> GetPatientAppointmentsAsync(int patientId);
        Task<IQueryable<Appointment>> GetFilterdAppointmentsAsync(PatientAppointmentsQueryModel queryModel, IQueryable<Appointment> patientAppointments);
    }
}

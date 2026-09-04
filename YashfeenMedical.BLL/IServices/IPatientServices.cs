using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.Invoices;
using YashfeenMedical.BLL.DTOs.MedicalFiles;
using YashfeenMedical.BLL.DTOs.MedicalRecords;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.DTOs.Prescriptions;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.IServices
{
    public interface IPatientServices : IEntityServices<int, PatientDto, PatientCreationDto, PatientUpdateDto>
    {
        Task<TPaginationQueryModel<PatientDto>> GetFilterdPatients(PatientQueryModel queryModel);
        Task<TPaginationQueryModel<AppointmentDto>> GetPaitentAppointments(PatientAppointmentsQueryModel queryModel, int paitentId);
        Task<TPaginationQueryModel<MedicalRecordDto>> GetPaitentMedicalRecords(PaginationQuery queryModel, int paitentId);
        Task<TPaginationQueryModel<PrescriptionDto>> GetPaitentPrescriptions(PaginationQuery queryModel, int paitentId);
        Task<TPaginationQueryModel<InvoiceDto>> GetPaitentInvoices(PaginationQuery queryModel, int paitentId);
        Task<TPaginationQueryModel<MedicalFileDto>> GetPaitentMedicalFiles(PaginationQuery queryModel, int paitentId);
        Task<string> TogglePatientActivitiy(int patientId);
    };
}

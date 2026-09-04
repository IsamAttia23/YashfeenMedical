using MapsterMapper;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.Invoices;
using YashfeenMedical.BLL.DTOs.MedicalFiles;
using YashfeenMedical.BLL.DTOs.MedicalRecords;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.DTOs.Prescriptions;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.BLL.Services
{
    public class PatientServices : TEntityService<Patient, int, PatientDto, PatientCreationDto, PatientUpdateDto>, IPatientServices
    {
        private readonly IPatientRepository _repository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMedicalFileRepository _medicalFileRepository;
        private readonly IMapper _mapper;

        public PatientServices(IPatientRepository repository, IMapper mapper
            , IAppointmentRepository appointmentRepository, IMedicalRecordRepository medicalRecordRepository,
              IPrescriptionRepository prescriptionRepository, IInvoiceRepository invoiceRepository,
              IMedicalFileRepository medicalFileRepository) : base(repository, mapper)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _prescriptionRepository = prescriptionRepository;
            _invoiceRepository = invoiceRepository;
            _medicalFileRepository = medicalFileRepository;
            _mapper = mapper;
        }

        public async Task<TPaginationQueryModel<PatientDto>> GetFilterdPatients(PatientQueryModel queryModel)
        {
            var paggedPatients = await _repository.GetFilteredPatientsWithPaginationAsync(queryModel);

            var result = _mapper.Map<TPaginationQueryModel<PatientDto>>(paggedPatients);

            return result;
        }

        public async Task<TPaginationQueryModel<InvoiceDto>> GetPaitentInvoices(PaginationQuery queryModel, int paitentId)
        {
            var invoices = await _invoiceRepository.GetInvoicesByPatientId(paitentId);
            var paggedList = await _invoiceRepository.GetPaggedList(invoices, queryModel);
            var result = _mapper.Map<TPaginationQueryModel<InvoiceDto>>(paggedList);

            return result;
        }

        public async Task<TPaginationQueryModel<AppointmentDto>> GetPaitentAppointments(PatientAppointmentsQueryModel queryModel, int paitentId)
        {
            var patientAppointments = await _appointmentRepository.GetPatientAppointmentsAsync(paitentId);
            var filterdAppointments = await _appointmentRepository.GetFilterdAppointmentsAsync(queryModel, patientAppointments);
            var paggedList = await _appointmentRepository.GetPaggedList(filterdAppointments, queryModel);
            var result = _mapper.Map<TPaginationQueryModel<AppointmentDto>>(paggedList);

            return result;
        }

        public async Task<TPaginationQueryModel<MedicalRecordDto>> GetPaitentMedicalRecords(PaginationQuery queryModel, int paitentId)
        {
            var medicalRecords = await _medicalRecordRepository.GetByPatientId(paitentId);
            var paggedList = await _medicalRecordRepository.GetPaggedList(medicalRecords, queryModel);
            var result = _mapper.Map<TPaginationQueryModel<MedicalRecordDto>>(paggedList);

            return result;
        }

        public async Task<TPaginationQueryModel<MedicalFileDto>> GetPaitentMedicalFiles(PaginationQuery queryModel, int paitentId)
        {
            var medicalFiles = await _medicalFileRepository.GetMedicalFileByPatientId(paitentId);
            var paggedList = await _medicalFileRepository.GetPaggedList(medicalFiles, queryModel);
            var result = _mapper.Map<TPaginationQueryModel<MedicalFileDto>>(paggedList);

            return result;
        }

        public async Task<TPaginationQueryModel<PrescriptionDto>> GetPaitentPrescriptions(PaginationQuery queryModel, int paitentId)
        {
            var prescriptions = await _prescriptionRepository.GetPrescriptionsByPatientId(paitentId);
            var paggedList = await _prescriptionRepository.GetPaggedList(prescriptions, queryModel);
            var result = _mapper.Map<TPaginationQueryModel<PrescriptionDto>>(paggedList);

            return result;
        }
    }
}

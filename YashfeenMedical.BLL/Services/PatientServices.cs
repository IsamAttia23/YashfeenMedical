using MapsterMapper;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.Patients;
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
        private readonly IMapper _mapper;

        public PatientServices(IPatientRepository repository, IMapper mapper
            , IAppointmentRepository appointmentRepository) : base(repository, mapper)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<TPaginationQueryModel<PatientDto>> GetFilterdPatients(PatientQueryModel queryModel)
        {
            var ordersPaged = await _repository.GetFilteredPatientsWithPaginationAsync(queryModel);

            var result = _mapper.Map<TPaginationQueryModel<PatientDto>>(ordersPaged);

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
    }
}

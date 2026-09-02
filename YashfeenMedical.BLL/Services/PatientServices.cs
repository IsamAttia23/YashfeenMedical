using MapsterMapper;
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
        private readonly IMapper _mapper;

        public PatientServices(IPatientRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TPaginationQueryModel<PatientDto>> GetFilterdOrders(PatientQueryModel queryModel, PaginationQuery query)
        {
            var ordersPaged = await _repository.GetFilteredPatientsWithPaginationAsync(queryModel, query);

            var result = _mapper.Map<TPaginationQueryModel<PatientDto>>(ordersPaged);

            return result;
        }
    }
}

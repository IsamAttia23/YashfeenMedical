using Mapster;
using MapsterMapper;
using YashfeenMedical.BLL.DTOs.MedicalRecords;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.BLL.Services
{
    public class MedicalRecordServices : TEntityService<MedicalRecord, int, MedicalRecordDto, MedicalRecordCreationDto, MedicalRecordUpdateDto>, IMedicalRecordServices
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly IPaginationServices _paginationServices;

        public MedicalRecordServices(IMedicalRecordRepository repository, IMapper mapper, IPaginationServices paginationServices) : base(repository, mapper, paginationServices)
        {
            _repository = repository;
            _paginationServices = paginationServices;
        }

        public async Task<TPaginationQueryModel<MedicalRecordDto>> GetMedicalRecordsByPatientId(PaginationQuery queryModel, int patientId)
        {
            var records = await _repository.GetByPatientId(patientId);
            var recrodsDto =records.ProjectToType<MedicalRecordDto>();
            var paginatedRecords = await _paginationServices.GetPaggedList(recrodsDto,queryModel);

           return paginatedRecords;
        }

        // For now no additional behavior beyond TEntityService; add methods as needed.
    }
}

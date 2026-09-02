using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.IServices
{
    public interface IPatientServices : IEntityServices<int, PatientDto, PatientCreationDto, PatientUpdateDto>
    {
        Task<TPaginationQueryModel<PatientDto>> GetFilterdOrders(PatientQueryModel queryModel, PaginationQuery query);
    }
}

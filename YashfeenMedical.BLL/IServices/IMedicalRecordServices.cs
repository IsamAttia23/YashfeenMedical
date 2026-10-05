using YashfeenMedical.BLL.DTOs.MedicalRecords;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.IServices
{
    public interface IMedicalRecordServices : IEntityServices<int, MedicalRecordDto, MedicalRecordCreationDto, MedicalRecordUpdateDto>
    {
        Task<TPaginationQueryModel<MedicalRecordDto>> GetMedicalRecordsByPatientId(PaginationQuery queryModel, int patientId);
    }
}

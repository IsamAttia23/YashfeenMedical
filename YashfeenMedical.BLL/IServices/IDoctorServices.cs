using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Doctors;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.BLL.IServices
{
    public interface IDoctorServices : IEntityServices<int, DoctorDto, DoctorCreationDto, DoctorUpdateDto>
    {
        Task<TPaginationQueryModel<DoctorDto>> GetFilteredDoctorsWithPaginationAsync(DoctorQueryModel queryModel);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Specialties;

namespace YashfeenMedical.BLL.IServices
{
    public interface ISpecialtyServices : IEntityServices<int, SpecialtyDto,SpecialtyCreationDto,SpecialtyUpdateDto>
    {
    }
}

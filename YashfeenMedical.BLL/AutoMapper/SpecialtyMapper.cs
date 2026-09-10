using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Specialties;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.BLL.AutoMapper
{
    public class SpecialtyMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Specialty, SpecialtyDto>();
        }
    }
}

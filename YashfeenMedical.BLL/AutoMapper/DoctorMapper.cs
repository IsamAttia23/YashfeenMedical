using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Doctors;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.BLL.AutoMapper
{
    public class DoctorMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Doctor, DoctorDto>()
                .Map(dest => dest.UserName, src => src.ApplicationUser.UserName)
                .Map(dest => dest.Email, src => src.ApplicationUser.Email)
                .Map(dest => dest.Phone, src => src.ApplicationUser.PhoneNumber)
                .Map(dest=> dest.DoctorSpecialties, src=> src.DoctorSpecialties.Select(ds=> ds.Specialty.Name));
        }
    }
}

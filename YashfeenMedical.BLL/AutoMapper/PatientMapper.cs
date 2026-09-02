using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.BLL.AutoMapper
{
    public class PatientMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Patient, PatientDto>()
                .Map(dest => dest.UserName, src => src.ApplicationUser.UserName)
                .Map(dest => dest.Email, src => src.ApplicationUser.Email)
                .Map(dest => dest.Phone, src => src.ApplicationUser.PhoneNumber);
        }
    }
}

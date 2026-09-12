using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.BLL.AutoMapper
{
    public class AppointmentMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Appointment,AppointmentDto>();
        }
    }
}

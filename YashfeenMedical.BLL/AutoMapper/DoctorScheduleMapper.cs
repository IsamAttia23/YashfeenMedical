using Mapster;
using System;
using YashfeenMedical.BLL.DTOs.DoctorSchedules;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.BLL.AutoMapper
{
    public class DoctorScheduleMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<DoctorSchedule, DoctorScheduleDto>()
                .Map(dest => dest.DayOfWeek, src => src.DayOfWeek.ToString());

            config.NewConfig<DoctorScheduleCreationDto, DoctorSchedule>()
                .Map(dest => dest.DayOfWeek, src => (ScheduleDayOfWeek)src.DayOfWeek);

            config.NewConfig<DoctorScheduleUpdateDto, DoctorSchedule>()
                .Map(dest => dest.DayOfWeek, src => (ScheduleDayOfWeek)src.DayOfWeek);
        }
    }
}

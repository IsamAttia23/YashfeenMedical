using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.AutoMapper;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.BLL.Services;
using YashfeenMedical.DAL;
using YashfeenMedical.Infrastructure;

namespace YashfeenMedical.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBllServices(this IServiceCollection services, IConfiguration configuration)
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(PatientMapper).Assembly);
            TypeAdapterConfig.GlobalSettings.Scan(typeof(DoctorMapper).Assembly);

            services.AddMapster();
            services.AddDalServices(configuration);
            services.AddInfrastructureServices(configuration);
            services.AddScoped<IPaginationServices, PaginationServices>();

            services.AddScoped<IAuthServices,AuthServices>();
            services.AddScoped<IPatientServices, PatientServices>();
            services.AddScoped<IDoctorServices, DoctorServices>();
            services.AddScoped<ISpecialtyServices, SpecialtyServices>();

            return services;
        }
    }
}

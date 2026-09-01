using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
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
            services.AddMapster();
            services.AddDalServices(configuration);
            services.AddInfrastructureServices(configuration);

            services.AddScoped<IAuthServices,AuthServices>();
            return services;
        }
    }
}

using Microsoft.AspNetCore.Builder;
using YashfeenMedical.Infrastructure.Middleware;

namespace YashfeenMedical.BLL.Extensions;

public static class BllExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}

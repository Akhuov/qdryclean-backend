using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces;
using QDryClean.Application.Common.Interfaces.Auth;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Infrastructure.Persistance;
using QDryClean.Infrastructure.Services;
using QDryClean.Infrastructure.Services.JWT;

namespace QDryClean.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DockerConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure());
            });

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            //Memory Cache (didn't use yet)
            services.AddMemoryCache();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQrCodeService, QrCodeService>();
            services.AddScoped<IBarCodeService, BarCodeService>();

            return services;
        }
    }
}
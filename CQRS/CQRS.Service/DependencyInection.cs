using CQRS.Data.Configurtion.Contracts;
using CQRS.Service.Identity;
using CQRS.Service.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.Service
{
    public static class DependencyInection
    {
        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(options => configuration.GetSection(nameof(JwtSettings)).Bind(options));
            services.AddScoped<IDateTime, DateTimeService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}

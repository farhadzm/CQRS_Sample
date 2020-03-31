using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CQRS.Application;
using CQRS.Application.Common.Mappings;
using CQRS.Data;
using CQRS.Data.Configurtion.Contracts;
using CQRS.Data.DataAccess;
using CQRS.Service;
using CQRS.Service.Jwt;
using CQRS.Web.Extensions;
using CQRS.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CQRS.Web
{
    public class Startup
    {
        private readonly JwtSettings _jwtSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _jwtSettings = Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        }
        public IConfiguration Configuration { get; set; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddData(Configuration);
            services.AddIdentityDbContext();
            services.AddApplication();            
            services.AddService(Configuration);
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(Lazy<>), typeof(Lazy<>));
            services.AddJwtAuthentication(_jwtSettings);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using CQRS.Data.DataAccess;
using CQRS.Domain.Entities;
using CQRS.Service.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Web.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddIdentityDbContext(this IServiceCollection services)
        {
            services.AddIdentity<Users, Roles>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }
        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretkey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                var encryptionkey = Encoding.UTF8.GetBytes(jwtSettings.EncryptKey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = true, //default : false
                    ValidIssuer = jwtSettings.Issure,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                };
                options.RequireHttpsMetadata = false;//todo set true to use https
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {

                        if (context.Exception != null)
                            throw new Exception("Authentication failed.", context.Exception);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {

                        await Task.CompletedTask;
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                    },
                    OnChallenge = context =>
                    {

                        if (context.AuthenticateFailure != null)
                            throw new Exception("Authenticate failure.", context.AuthenticateFailure);
                        throw new Exception("You are unauthorized to access this resource.");

                    }
                };
            });
        }
    }
}

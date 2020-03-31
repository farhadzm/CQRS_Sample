using CQRS.Data.Configurtion.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace CQRS.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = Convert.ToInt32(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        public int UserId { get; }
    }
}

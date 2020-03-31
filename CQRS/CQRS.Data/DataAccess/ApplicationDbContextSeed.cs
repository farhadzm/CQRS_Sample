using CQRS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Data.DataAccess
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<Users> userManager)
        {
            // Create default administrator
            var defaultUser = new Users { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (!userManager.Users.Any())
            {
                await userManager.CreateAsync(defaultUser, "Administrator1!");
            }
        }
    }
}

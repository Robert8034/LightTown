using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LightTown.Server.Data
{
    public static class ApplicationBuilderExtensions
    {
        public static void EnsureMigrated(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<LightTownServerContext>();

                dbContext.Database.Migrate();
            }
        }

    }
}
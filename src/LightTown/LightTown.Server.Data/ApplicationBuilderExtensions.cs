using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace LightTown.Server.Data
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Ensure that the database exists with all its tables and default data.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        public static void EnsureMigrated(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<LightTownServerContext>();

                var relationalDatabaseCreator =
                    (RelationalDatabaseCreator) dbContext.Database.GetService<IDatabaseCreator>();
                var tablesExists = relationalDatabaseCreator.HasTables();
                
                dbContext.Database.Migrate();

                if (!tablesExists)
                {
                    RoleManager<Role> roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
                    roleManager.CreateAsync(new Role("Administrator", Permissions.ALL, false));
                    roleManager.CreateAsync(new Role("Manager", Permissions.ALL));
                    roleManager.CreateAsync(new Role("Employee", Permissions.ALL));

                    UserManager<User> userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                    userManager.CreateAsync(new User("admin"), "admin");
                }
            }
        }
    }
}
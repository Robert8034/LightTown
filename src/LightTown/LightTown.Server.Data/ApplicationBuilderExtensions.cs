using System;
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

                var databaseExists = relationalDatabaseCreator.Exists() && relationalDatabaseCreator.HasTables();
                
                dbContext.Database.Migrate();

                if (!databaseExists)
                {
                    RoleManager<Role> roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
                    roleManager.CreateAsync(new Role("Administrator", Permissions.ALL, false)).Wait();
                    roleManager.CreateAsync(new Role("Manager", Permissions.CREATE_PROJECTS |
                                                                Permissions.MANAGE_PROJECTS |
                                                                Permissions.DELETE_PROJECTS |
                                                                Permissions.MANAGE_USERS |
                                                                Permissions.VIEW_ALL_PROJECTS)).Wait();
                    roleManager.CreateAsync(new Role("Employee", Permissions.VIEW_ALL_PROJECTS)).Wait();

                    UserManager<User> userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                    userManager.CreateAsync(new User("admin"), "admin").Wait();
                }
            }
        }
    }
}
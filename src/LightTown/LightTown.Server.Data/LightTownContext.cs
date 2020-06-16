using LightTown.Core.Domain.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using LightTown.Core.Domain.Users;
using LightTown.Server.Data.Mapping;
using LightTown.Server.Config;

namespace LightTown.Server.Data
{
    public class LightTownContext : IdentityDbContext<User, Role, int>
    {
        public LightTownContext()
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var configurations = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
                (t.BaseType?.IsGenericType ?? false) && t.BaseType?.GetGenericTypeDefinition() == typeof(EntityMappingConfiguration<>));

            foreach (Type configurationType in configurations)
            {
                var configuration = (IEntityMappingConfiguration)Activator.CreateInstance(configurationType);
                configuration.ApplyConfiguration(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"User ID={Config.Config.PostgresUserId};" +
                                     $"Password={Config.Config.PostgresPassword};" +
                                     $"Host={Config.Config.PostgresHost};" +
                                     $"Port={Config.Config.PostgresPort};" +
                                     $"Database={Config.Config.PostgresDatabase};");

            //TODO remove this?
            optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }
}

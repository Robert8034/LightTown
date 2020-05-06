using LightTown.Core.Domain.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using LightTown.Core.Domain.Users;
using LightTown.Server.Data.Mapping;
using Microsoft.Extensions.Logging;

namespace LightTown.Server.Data
{
    public class LightTownServerContext : IdentityDbContext<User, Role, int>
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public LightTownServerContext() { }

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
            //TODO Add credentials in config file

            optionsBuilder.UseLoggerFactory(MyLoggerFactory);

            optionsBuilder.UseNpgsql("User ID=lighttown;Password=pHmGfPMJ8LpV4CnPxZRy6wKTqAXdxi8nUKHw;Host=localhost;Port=5432;Database=LightTown;");

            optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.EnableSensitiveDataLogging(true);

        }
    }
}

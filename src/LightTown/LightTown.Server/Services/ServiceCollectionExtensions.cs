﻿using LightTown.Server.Services.Projects;
using Microsoft.Extensions.DependencyInjection;

namespace LightTown.Server.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IProjectService), typeof(ProjectService));
        }
    }
}
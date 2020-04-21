using LightTown.Core.Data;
using LightTown.Core.Domain.Projects;
using Microsoft.Extensions.DependencyInjection;

namespace LightTown.Server.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddScoped(typeof(IRepository<Project>), typeof(Repository<Project>));
        }
    }
}

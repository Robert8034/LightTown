using LightTown.Server.Services.Messages;
using LightTown.Server.Services.Projects;
using LightTown.Server.Services.Tags;
using LightTown.Server.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LightTown.Server.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IProjectService, ProjectService>();
            serviceCollection.AddScoped<IProjectMemberService, ProjectMemberService>();
            serviceCollection.AddScoped<ITagService, TagService>();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IMessageService, MessageService>();
            serviceCollection.AddScoped<IProjectLikeService, ProjectLikeService>();
        }
    }
}

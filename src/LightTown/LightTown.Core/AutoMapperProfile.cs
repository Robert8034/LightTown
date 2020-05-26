using AutoMapper;
using LightTown.Core.Domain.Projects;
using LightTown.Core.Domain.Tags;
using LightTown.Core.Domain.Users;

namespace LightTown.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, Models.Users.User>();
            CreateMap<Project, Models.Projects.Project>();
            CreateMap<ProjectMember, Models.Projects.ProjectMember>();
            CreateMap<Tag, Models.Tags.Tag>();
        }
    }
}

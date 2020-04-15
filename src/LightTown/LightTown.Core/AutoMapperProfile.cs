using AutoMapper;
using LightTown.Core.Domain.Users;

namespace LightTown.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, Models.Users.User>();
        }
    }
}

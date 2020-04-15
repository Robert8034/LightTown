using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace LightTown.Core.Models.Users
{
    [AutoMap(typeof(Domain.Users.User))]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        [SourceMember(nameof(Domain.Users.User.EmailConfirmed))]
        public bool IsEmailConfirmed { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using LightTown.Core.Models.Tags;

namespace LightTown.Core.Models.Users
{
    [AutoMap(typeof(Domain.Users.User))]
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Email { get; set; }
        [SourceMember(nameof(Domain.Users.User.EmailConfirmed))]
        public bool IsEmailConfirmed { get; set; }
        public bool HasAvatar { get; set; }
        public string Fullname { get; set; }
        public string About { get; set; }
        public int Age { get; set; }
        public string Hometown { get; set; }
        public string Job { get; set; }
        public string AvatarFilename { get; set; }
        public List<int> TagIds { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
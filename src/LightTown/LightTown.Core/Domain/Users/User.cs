using System.Collections.Generic;
using LightTown.Core.Domain.Tags;
using Microsoft.AspNetCore.Identity;

namespace LightTown.Core.Domain.Users
{
    public class User : IdentityUser<int>
    {
        public string Fullname { get; set; }
        public string About { get; set; }
        public bool HasAvatar { get; set; }
        public int Age { get; set; }
        public string Hometown { get; set; }
        public string Job { get; set; }
        public List<Tag> Tags { get; set; }

        public User(string username)
        {
            base.UserName = username;
        }

        public User() { }
    }
}
﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LightTown.Core.Domain.Users
{
    public class User : IdentityUser<int>
    {
        public string Fullname { get; set; }
        public string About { get; set; }
        public bool HasAvatar { get; set; }
        public string Hometown { get; set; }
        public string Job { get; set; }
        public List<UserTag> Tags { get; set; }
        public string AvatarFilename { get; set; }

        public User(string username)
        {
            base.UserName = username;
        }

        public User() { }
    }
}
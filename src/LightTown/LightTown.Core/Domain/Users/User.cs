using System;
using LightTown.Core.Data;

namespace LightTown.Server.Core.Domain.Users
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
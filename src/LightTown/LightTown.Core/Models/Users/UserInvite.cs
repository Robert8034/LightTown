using System;
using AutoMapper;

namespace LightTown.Core.Models.Users
{
    [AutoMap(typeof(Domain.Users.UserInvite))]
    public class UserInvite
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string InviteCode { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
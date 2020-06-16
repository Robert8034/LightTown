using System;
using LightTown.Core.Data;

namespace LightTown.Core.Domain.Users
{
    public class UserInvite : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string InviteCode { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
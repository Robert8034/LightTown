using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Data;
using LightTown.Core.Domain.Tags;

namespace LightTown.Core.Domain.Users
{
    public class UserTag : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}

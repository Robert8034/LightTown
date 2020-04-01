using LightTown.Server.Core.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LightTown.Core.Domain.Roles
{
    public class Role : IdentityRole<int>
    {
        public Permissions Permissions { get; set; }

        public Role(string name, Permissions permissions)
        {
            base.Name = name;
            Permissions = permissions;
        }

        public Role() { }
    }
}
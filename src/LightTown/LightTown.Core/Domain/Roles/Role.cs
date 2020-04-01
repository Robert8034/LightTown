using Microsoft.AspNetCore.Identity;

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
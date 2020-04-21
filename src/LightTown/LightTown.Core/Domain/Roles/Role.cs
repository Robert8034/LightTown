using Microsoft.AspNetCore.Identity;

namespace LightTown.Core.Domain.Roles
{
    public class Role : IdentityRole<int>
    {
        public bool CanBeModified { get; set; }
        public Permissions Permissions { get; set; }

        public Role(string name, Permissions permissions, bool canBeModified = true)
        {
            CanBeModified = canBeModified;
            base.Name = name;
            Permissions = permissions;
        }

        public Role() { }
    }
}
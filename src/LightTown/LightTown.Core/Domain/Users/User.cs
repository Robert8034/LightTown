using Microsoft.AspNetCore.Identity;

namespace LightTown.Core.Domain.Users
{
    public class User : IdentityUser<int>
    {
        public User(string username)
        {
            base.UserName = username;
        }

        public User() { }
    }
}
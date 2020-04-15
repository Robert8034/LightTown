using LightTown.Core.Domain.Roles;
using Microsoft.AspNetCore.Mvc;

namespace LightTown.Server
{
    public class AuthorizationAttribute : TypeFilterAttribute
    {
        public AuthorizationAttribute(Permissions permission) : base(typeof(AuthenticationFilter))
        {
            Arguments = new object[] { permission };
        }
    }
}

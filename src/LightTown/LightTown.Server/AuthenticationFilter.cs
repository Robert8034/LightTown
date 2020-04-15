using System.Linq;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LightTown.Server
{
    public class AuthenticationFilter : IAuthorizationFilter
    {
        readonly Permissions _permission;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthenticationFilter(Permissions permission, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _permission = permission;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User == null || !context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
            }
            else
            {
                var user = _userManager.GetUserAsync(context.HttpContext.User).Result;

                if(user == null)
                    context.Result = new StatusCodeResult(401);

                var roleNames = _userManager.GetRolesAsync(user).Result;

                var roles = roleNames.Select(roleName => _roleManager.FindByNameAsync(roleName).Result);

                if(!roles.Any(role => role.Permissions.HasFlag(_permission)))
                    context.Result = new StatusCodeResult(403);
            }
        }
    }
}
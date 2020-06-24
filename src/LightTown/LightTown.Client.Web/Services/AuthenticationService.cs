using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Client.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace LightTown.Client.Web.Services
{
    public class AuthenticationService
    {
        private readonly IUserAuthService _userAuthService;
        private readonly NavigationManager _navigationManager;

        private string[] noAuthPages = 
        {
            "/login"
        };

        public AuthenticationService(IUserAuthService userAuthService, NavigationManager navigationManager)
        {
            _userAuthService = userAuthService;
            _navigationManager = navigationManager;
        }

        /// <summary>
        /// Returns true if the current user has access to the page based on if the user is logged in or not.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool HasAccessToPage(string url)
        {
            if (_userAuthService.IsAuthorized())
                return true;

            var relativeUrl = "/" + _navigationManager.ToBaseRelativePath(url);

            return noAuthPages.Contains(relativeUrl);
        }
    }
}

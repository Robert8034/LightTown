using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LightTown.Client.Services.Users
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IJSRuntime _ijsRuntime;
        private readonly NavigationManager _navigationManager;
        private readonly IUserDataService _userDataService;

        public Func<Task> OnAuthorizationChange { get; set; }

        public UserAuthService(IJSRuntime ijsRuntime, NavigationManager navigationManager, IUserDataService userDataService)
        {
            _ijsRuntime = ijsRuntime;
            _navigationManager = navigationManager;
            _userDataService = userDataService;
        }

        /// <summary>
        /// Returns if a user is authorized and if the cookie is correct. Note that this information is gathered from client sided data.
        /// </summary>
        /// <returns></returns>
        public bool IsAuthorized()
        {
            return _userDataService.GetCurrentUser() != null;
        }

        /// <summary>
        /// Try to load user authorization cookie from local storage.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if valid info has been found.</returns>
        public async Task<bool> TryLoadAuthentication()
        {
            var cookieString = await _ijsRuntime.InvokeAsync<string>("getCookies");

            if (string.IsNullOrEmpty(cookieString))
                return false;

            var cookieContainer = new CookieContainer();
            cookieContainer.SetCookies(new Uri(_navigationManager.BaseUri), cookieString);

            var cookieCollection = cookieContainer.GetCookies(new Uri(_navigationManager.BaseUri));

            var cookie = cookieCollection[".AspNetCore.Identity.Application"];

            if (cookie == null)
                return false;

            if (cookie.Expired)
                return false;

            return true;
        }

        /// <summary>
        /// Unload the current user from the <see cref="UserDataService"/> cache and removes authentication cookie.
        /// </summary>
        public async void UnloadAuthentication()
        {
            _userDataService.UnloadData();

            await _ijsRuntime.InvokeVoidAsync("unsetCookies");

            OnAuthorizationChange?.Invoke();
        }
    }
}

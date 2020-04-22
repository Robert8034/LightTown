using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LightTown.Core.Models.Users;
using LightTown.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LightTown.Client.Services.Users
{
    public class UserSessionService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _ijsRuntime;
        private readonly NavigationManager _navigationManager;

        public Func<Task> OnAuthorizationChange { get; set; }

        private User _currentUser;

        public UserSessionService(HttpClient httpClient, IJSRuntime ijsRuntime, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _ijsRuntime = ijsRuntime;
            _navigationManager = navigationManager;
        }

        /// <summary>
        /// Set the current user (or null) and invoke the authorization changed event for the navigation menu to update.
        /// </summary>
        /// <param name="user"></param>
        public void SetCurrentUser(User user)
        {
            _currentUser = user;

            OnAuthorizationChange?.Invoke();
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        /// <summary>
        /// Returns if a user is authorized and if the cookie is correct. Note that this information is gathered from client sided data.
        /// </summary>
        /// <returns></returns>
        public bool IsAuthorized()
        {
            return _currentUser != null;
        }

        /// <summary>
        /// Try to load user authorization cookie from local storage.
        /// </summary>
        /// <returns>Returns true if valid info has been found.</returns>
        public async Task<bool> TryLoadLocalUser()
        {
            var cookieString = await _ijsRuntime.InvokeAsync<string>("getCookies");

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
        /// Load current user data from the server into cache.
        /// </summary>
        /// <returns></returns>
        public async Task LoadUser()
        {
            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/users/@me");

            SetCurrentUser(result.GetData<User>());
        }

        /// <summary>
        /// Unload the current user from the cache and removes authentication cookie.
        /// </summary>
        public async void UnloadUser()
        {
            SetCurrentUser(null);

            await _ijsRuntime.InvokeVoidAsync("unsetCookies");

            OnAuthorizationChange?.Invoke();
        }
    }
}

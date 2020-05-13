using System;
using System.Threading.Tasks;

namespace LightTown.Client.Services.Users
{
    public interface IUserAuthService
    {
        Func<Task> OnAuthorizationChange { get; set; }

        /// <summary>
        /// Returns if a user is authorized and if the cookie is correct. Note that this information is gathered from client sided data.
        /// </summary>
        /// <returns></returns>
        bool IsAuthorized();

        /// <summary>
        /// Try to load user authorization cookie from local storage.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if valid info has been found.</returns>
        Task<bool> TryLoadAuthentication();

        /// <summary>
        /// Unload the current user from the <see cref="UserDataService"/> cache and removes authentication cookie.
        /// </summary>
        void UnloadAuthentication();
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using LightTown.Core.Models.Projects;
using LightTown.Core.Models.Users;

namespace LightTown.Client.Services.Users
{
    public interface IUserDataService
    {
        /// <summary>
        /// Load current user data, expecting a valid authorization cookie to be set.
        /// </summary>
        Task LoadData();

        /// <summary>
        /// Unload all data including the current user.
        /// </summary>
        void UnloadData();

        /// <summary>
        /// Get the current user object or <see langword="null"/> if no user is loaded.
        /// </summary>
        /// <returns></returns>
        User GetCurrentUser();

        /// <summary>
        /// Get the list of available projects.
        /// </summary>
        /// <returns></returns>
        Task<List<Project>> GetProjects();
    }
}
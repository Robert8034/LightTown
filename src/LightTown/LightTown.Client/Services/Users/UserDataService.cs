using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LightTown.Core;
using LightTown.Core.Models.Projects;
using LightTown.Core.Models.Tags;
using LightTown.Core.Models.Users;

namespace LightTown.Client.Services.Users
{
    /// <summary>
    /// Service that holds data of the current user which means that less API calls have to be made.
    /// <para>
    /// It's expected that this service will not be used when the user isn't authorized.
    /// </para>
    /// </summary>
    public class UserDataService : IUserDataService
    {
        private readonly HttpClient _httpClient;

        private User _currentUser;
        private List<Project> _projects;
        private List<Tag> _tags;

        //lock objects so a second thread cant access the object when it is being loaded (using httpclient) by another thread.
        private readonly SemaphoreSlim _userLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _projectsLock = new SemaphoreSlim(1, 1);

        public UserDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Load current user data, expecting a valid authorization cookie to be set.
        /// </summary>
        public async Task LoadData()
        {
            try
            {
                await _userLock.WaitAsync();

                ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/users/@me");

                _currentUser = result.GetData<User>();
            }
            catch (Exception e)
            {
                //TODO create ErrorService that shows error on page
                throw e;
            }
            finally
            {
                _userLock.Release();
            }
        }

        /// <summary>
        /// Unload all data including the current user.
        /// </summary>
        public void UnloadData()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Get the current user object or <see langword="null"/> if no user is loaded.
        /// </summary>
        /// <returns></returns>
        public User GetCurrentUser()
        {
            return _currentUser;
        }

        /// <summary>
        /// Get the list of available projects.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Project>> GetProjects()
        {
            if (_projects == null)
            {
                await _projectsLock.WaitAsync(TimeSpan.FromSeconds(5));

                try
                {
                    ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects");

                    _projects = result.GetData<List<Project>>();
                }
                catch (Exception e)
                {
                    //TODO use ErrorService to show error
                    throw e;
                }
                finally
                {
                    _projectsLock.Release();
                }
            }
            
            return _projects;
        }
    }
}
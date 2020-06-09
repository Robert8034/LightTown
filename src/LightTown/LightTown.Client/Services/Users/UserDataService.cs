using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LightTown.Client.Services.Popups;
using LightTown.Core;
using LightTown.Core.Models.Projects;
using LightTown.Core.Models.Roles;
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
        private readonly IPopupService<BlazorPopupService.Popup> _alertService;

        public Func<Task> OnUserDataChange { get; set; }

        private User _currentUser;
        private Dictionary<int, Project> _projects;
        private Dictionary<int, Role> _roles;
        private Dictionary<int, User> _users;
        private Dictionary<int, Tag> _tags;

        //lock objects so a second thread cant access the object when it is being loaded (using httpclient) by another thread.
        private readonly SemaphoreSlim _currentUserLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _usersLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _projectsLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _tagsLock = new SemaphoreSlim(1,1);
        private readonly SemaphoreSlim _rolesLock = new SemaphoreSlim(1,1);

        public UserDataService(HttpClient httpClient, IPopupService<BlazorPopupService.Popup> alertService)
        {
            _httpClient = httpClient;
            _alertService = alertService;
            _projects = new Dictionary<int, Project>();
            _users = new Dictionary<int, User>();
            _tags = new Dictionary<int, Tag>();
            _roles = new Dictionary<int, Role>();
        }

        /// <summary>
        /// Load current user data, expecting a valid authorization cookie to be set.
        /// </summary>
        public async Task LoadData()
        {
            try
            {
                await _currentUserLock.WaitAsync();

                ApiResult userResult = await _httpClient.GetJsonAsync<ApiResult>("api/users/@me");

                _currentUser = userResult.GetData<User>();

                ApiResult tagsResult = await _httpClient.GetJsonAsync<ApiResult>("api/tags");

                var tags = tagsResult.GetData<List<Tag>>();
                
                foreach (Tag tag in tags)
                {
                    _tags[tag.Id] = tag;
                }

                await FillUser(_currentUser);
            }
            catch (Exception e)
            {
                _alertService?.ShowErrorPopup(true, null, "Error getting user data: " + e.Message);
            }
            finally
            {
                _currentUserLock.Release();
            }

            if(OnUserDataChange != null)
                await OnUserDataChange.Invoke();
        }
        
        /// <summary>
        /// Unload all data including the current user.
        /// </summary>
        public async Task UnloadData()
        {
            _currentUser = null;

            if (OnUserDataChange != null)
                await OnUserDataChange.Invoke();
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
        /// Set the current user object or <see langword="null"/> if no user is loaded.
        /// </summary>
        /// <returns></returns>
        public void SetCurrentUser(User user)
        {
            _currentUser = user;
        }

        public List<Tag> SearchTags(string name, List<Tag> excludeList = null)
        {
            int maxResults = 10;

            List<Tag> results = new List<Tag>();

            //filter tags based on exclude list
            List<Tag> filteredTags = _tags.Values.Where(tag => !excludeList?.Contains(tag) ?? true).ToList();

            //search for tag with the exact name or with name to lower
            if(filteredTags.Any(tag => tag.Name == name))
                results.Add(filteredTags.First(tag => tag.Name == name));
            else if (filteredTags.Any(tag => tag.Name.ToLower() == name.ToLower()))
                results.Add(filteredTags.First(tag => tag.Name.ToLower() == name.ToLower()));

            //search for tags that start with name (don't include tags already in the results list)
            results.AddRange(filteredTags.Where(tag => !results.Contains(tag) &&
                                                       tag.Name.ToLower().StartsWith(name.ToLower())).ToList());

            //return the first 10 results if 10 has already been reached
            if (results.Count >= maxResults)
                return results.Take(maxResults).ToList();

            //search for tags that contain name (don't include tags already in the results list)
            results.AddRange(filteredTags.Where(tag => !results.Contains(tag) &&
                                                       tag.Name.ToLower().Contains(name.ToLower())));

            //return the first 10 results
            return results.Take(maxResults).ToList();
        }

        /// <summary>
        /// Add tags to the cache if they don't already exist. Returns a list of tags from input, replaced with cached objects for already existing tags.
        /// </summary>
        /// <param name="tags"></param>
        public IEnumerable<Tag> SetTags(List<Tag> tags)
        {
            foreach (var tag in tags)
            {
                if (!_tags.ContainsKey(tag.Id))
                {
                    _tags[tag.Id] = tag;
                    yield return tag;
                }
                else
                {
                    yield return _tags[tag.Id];
                }
            }
        }

        /// <summary>
        /// Get the list of available projects.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Project>> GetProjects()
        {
            if (_projects.Count == 0)
            {
                await _projectsLock.WaitAsync();

                try
                {
                    ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects");

                    _projects = result.GetData<List<Project>>()
                        .ToDictionary(project => project.Id, project => project);

                    foreach (Project project in _projects.Values)
                    {
                        await FillProject(project);
                    }
                }
                catch (Exception e)
                {
                    _alertService?.ShowErrorPopup(true, null, "Error getting projects: " + e.Message);
                }
                finally
                {
                    _projectsLock.Release();
                }
            }
            
            return _projects?.Values.ToList();
        }

        /// <summary>
        /// Get a project, will get it from the server if it doesn't exist in the cache. Returns <see langword="null"/> on error or if it doesn't exist.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<Project> GetProject(int projectId)
        {
            if (!_projects.ContainsKey(projectId))
            {
                await _projectsLock.WaitAsync();

                try
                {
                    ApiResult result = await _httpClient.GetJsonAsync<ApiResult>($"api/projects/{projectId}");

                    var project = result.GetData<Project>();

                    _projects[projectId] = project;

                    await FillProject(project);
                }
                catch (Exception e)
                {
                    _alertService?.ShowErrorPopup(true, null, "Error getting project: " + e.Message);

                    return null;
                }
                finally
                {
                    _projectsLock.Release();
                }
            }

            return _projects[projectId];
        }

        /// <summary>
        /// Get a user, will get it from the server if it doesn't exist in the cache.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUser(int userId)
        {
            if (userId == _currentUser?.Id)
                return _currentUser;

            if (!_users.ContainsKey(userId))
            {
                await _usersLock.WaitAsync();

                try
                {
                    ApiResult result = await _httpClient.GetJsonAsync<ApiResult>($"api/users/{userId}");

                    _users[userId] = result.GetData<User>();

                    await FillUser(_users[userId]);
                }
                catch (Exception e)
                {
                    _alertService?.ShowErrorPopup(true, null, "Error getting user: " + e.Message);

                    return null;
                }
                finally
                {
                    _usersLock.Release();
                }
            }

            return _users[userId];
        }

        /// <summary>
        /// Get a list of members of a project, will get it from the server if it doesn't exist in the cache. Returns <see langword="null"/> on error or if the project doesn't exist.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<User>> GetProjectMembers(int projectId)
        {
            var project = await GetProject(projectId);

            if (project == null)
                return null;

            if (project.Members == null)
            {
                try
                {
                    ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects/" + projectId + "/members");
                    project.Members = result.GetData<List<User>>();

                    foreach (User projectMember in project.Members)
                    {
                        _users[projectMember.Id] = projectMember;
                    }
                }
                catch (Exception e)
                {
                    _alertService?.ShowErrorPopup(true, null, "Error getting project members: " + e.Message);
                }
            }

            return project.Members;
        }

        /// <summary>
        /// Get a tag, will get it from the server if it doesn't exist in the cache. Returns <see langword="null"/> on error or if it doesn't exist.
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns>One specific tag.</returns>
        public async Task<Tag> GetTag(int tagId)
        {
            if (!_tags.ContainsKey(tagId))
            {
                await _tagsLock.WaitAsync();

                try
                {
                    ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/tags/" + tagId);
                    _tags[tagId] = result.GetData<Tag>();
                }
                catch (Exception e)
                {
                    _alertService?.ShowErrorPopup(true, null, "Error getting tags: " + e.Message);

                    return null;
                }
                finally
                {
                    _tagsLock.Release();
                }
            }
            return _tags[tagId];
        }

        /// <summary>
        /// Get a list of project tags. Returns <see langword="null"/> on error or if the project doesn't exist.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A list of project tags.</returns>
        public async Task<List<Tag>> GetProjectTags(int projectId)
        {
            var projectTags = new List<Tag>();

            var project = await GetProject(projectId);

            if (project == null)
                return null;

            foreach (var tagId in project.TagIds)
            {
                var tag = await GetTag(tagId);

                if(tag != null)
                    projectTags.Add(tag);
            }

            return projectTags;
        }

        /// <summary>
        /// Get a list of tags that a user has.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A list of project tags.</returns>
        public async Task<List<Tag>> GetUserTags(int userId)
        {
            var userTags = new List<Tag>();

            var user = await GetUser(userId);

            foreach (var tagId in user.TagIds)
            {
                var tag = await GetTag(tagId);

                if (tag != null)
                    userTags.Add(tag);
            }

            return userTags;
        }
        
        /// <summary>
        /// Fill a project with tags (NOT members!).
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private async Task FillProject(Project project)
        {
            project.Tags = new List<Tag>();

            if (project.TagIds == null)
                return;

            foreach (int tagId in project.TagIds)
            {
                var tag = await GetTag(tagId);

                if (tag != null)
                    project.Tags.Add(tag);
            }
        }

        /// <summary>
        /// Fill a user with tags.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task FillUser(User user)
        {
            user.Tags = new List<Tag>();

            if (user.TagIds == null)
                return;

            foreach (int tagId in user.TagIds)
            {
                var tag = await GetTag(tagId);

                if (tag != null)
                    user.Tags.Add(tag);
            }
        }

        /// <summary>
        /// Get the list of roles.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetRoles()
        {
            if (_roles.Count == 0)
            {
                await _rolesLock.WaitAsync();

                try
                {
                    ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/roles");

                    _roles = result.GetData<List<Role>>()
                        .ToDictionary(role => role.Id, role => role);
                }
                catch (Exception e)
                {
                    _alertService?.ShowErrorPopup(true, null, "Error getting roles: " + e.Message);
                }
                finally
                {
                    _rolesLock.Release();
                }
            }

            return _roles?.Values.ToList();
        }
    }
}
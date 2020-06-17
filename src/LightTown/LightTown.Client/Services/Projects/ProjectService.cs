using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LightTown.Client.Services.Popups;
using LightTown.Client.Services.Users;
using LightTown.Core;
using LightTown.Core.Models.Messages;
using LightTown.Core.Models.Projects;
using LightTown.Core.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace LightTown.Client.Services.Projects
{
    //TODO add error handling in methods
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;
        private readonly IPopupService<BlazorPopupService.Popup> _alertService;
        private readonly IUserDataService _userDataService;

        public ProjectService(HttpClient httpClient, IUserDataService userDataService, IPopupService<BlazorPopupService.Popup> alertService)
        {
            _httpClient = httpClient;
            _userDataService = userDataService;
            _alertService = alertService;
        }

        ///// <summary>
        ///// Gets all projects that exist in the database.
        ///// <para>
        ///// <returns>Returns a list of <see cref="Project"></see>.</returns>
        ///// </para>
        ///// </summary>
        //public async Task<List<Project>> GetProjects()
        //{

        //    ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects");

        //    return result.GetData<List<Project>>();

        //}

        /// <summary>
        /// Creates a new project.
        /// <param name="projectName"></param>
        /// <param name="projectDescription"></param>
        /// <para>
        /// <returns>Returns the newly created <see cref="Project"></see>.</returns>
        /// </para>
        /// </summary>
        public async Task<Project> CreateProject(string projectName, string projectDescription)
        {
            ApiResult result;
            try
            {
                result = await _httpClient.PostJsonAsync<ApiResult>("api/projects", new
                    {
                        projectName,
                        projectDescription
                    }
                );
            }
            catch (Exception e)
            {
                _alertService?.ShowErrorPopup(true, null, "You are not allowed to create a new project.");
                throw;
            }

            return result.GetData<Project>();
        }

        ///// <summary>
        ///// Gets one specific project based on <paramref name="projectId"/>.
        ///// <param name="projectId"></param>
        ///// <para></para>
        ///// <returns>Returns the target <see cref="Project"></see> or <see langword="null"></see> if not found.</returns>
        ///// </summary>
        //public async Task<Project> GetProject(int projectId)
        //{
        //    ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects/" + projectId);

        //    Project test = result.GetData<Project>();

        //    Console.WriteLine(test.Members[0].Username);

        //    return result.GetData<Project>();
        //}

        /// <summary>
        /// Removes target <see cref="User"/> using <paramref name="userId"/> of the target <see cref="Project"/> using <paramref name="projectId"/>.
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <para></para>
        /// <returns>Returns <see langword="true"/> if successful or <see langword="false"/> if unsuccessful.</returns>
        /// </summary>
        public async Task<bool> RemoveMember(int projectId, int userId)
        {
            try
            {
                ApiResult result =
                    await _httpClient.DeleteJsonAsync<ApiResult>("api/projects/" + projectId + "/members/" + userId);
            }
            catch (Exception e)
            {
                _alertService?.ShowErrorPopup(true, null, "You are not allowed to remove members from this project.");
            }

            return true;
        }

        /// <summary>
        /// Adds target <see cref="User"/> using <paramref name="userId"/> to the target <see cref="Project"/> using <paramref name="projectId"/>.
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <para></para>
        /// <returns>Returns <see langword="true"/> if successful or <see langword="false"/> if unsuccessful.</returns>
        /// </summary>
        public async Task<bool> AddMember(int userId, int projectId)
        {
            try
            {
                ApiResult result = await _httpClient.PutJson<ApiResult>("api/projects/" + projectId + "/members/" + userId,
                    new
                    {
                        userId,
                        projectId
                    });
            }
            catch (Exception e)
            {
                _alertService?.ShowErrorPopup(true, null, "You are not allowed to create a new project.");
            }

            return true;
        }

        /// <summary>
        /// Gets a list of <see cref="User"/> using <paramref name="projectId"/> from a <see cref="Project"/>.
        /// <param name="projectId"></param>
        /// <para></para>
        /// <returns>Returns a list of <see cref="User"/> if successful or <see langword="null"/> if unsuccessful.</returns>
        /// </summary>
        public async Task<List<User>> GetProjectMembers(int projectId)
        {
            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects/" + projectId + "/members");

            return result.GetData<List<User>>();
        }

        /// <summary>
        /// Gets a list of <see cref="Project"/> using <paramref name="searchValue"/>.
        /// <param name="projectId"></param>
        /// <para></para>
        /// <returns>Returns a list of <see cref="Project"/> if successful or <see langword="null"/> if unsuccessful.</returns>
        /// </summary>
        public async Task<List<Project>> SearchProjects(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue)) return new List<Project>();

            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects/search/" + searchValue);

            return result.GetData<List<Project>>();
        }

        public async Task<bool> PostProjectMessage(int projectId, string content, string title)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(title))
            {
                return false;
            }

            try
            {
                ApiResult result = await _httpClient.PutJson<ApiResult>("api/projects/" + projectId + "/messages", new
                {
                    title,
                    content
                });
            }
            catch (Exception e)
            {
                _alertService?.ShowErrorPopup(true, null, "You are not a member of this project, so you can not create a new message.");
                throw;
            }

            return true;

        }

        public async Task<List<Message>> GetProjectMessages(int projectId)
        {
            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects/" + projectId + "/messages");

            return result.GetData<List<Message>>();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LightTown.Core;
using LightTown.Core.Models.Projects;
using LightTown.Core.Models.Users;

namespace LightTown.Client.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;

        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets all projects that excist in the database.
        /// <para>
        /// <returns>Returns a list of <see cref="Project"></see>.</returns>
        /// </para>
        /// </summary>
        public async Task<List<Project>> GetProjects()
        {

            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects");

            return result.GetData<List<Project>>();

        }

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
            ApiResult result = await _httpClient.PostJsonAsync<ApiResult>("api/projects", new
                {
                    projectName,
                    projectDescription
                }
            );

            return result.GetData<Project>();
        }

        /// <summary>
        /// Gets one specific project based on <paramref name="projectId"/>.
        /// <param name="projectId"></param>
        /// <para></para>
        /// <returns>Returns the target <see cref="Project"></see> or <see langword="null"></see> if not found.</returns>
        /// </summary>
        public async Task<Project> GetProject(int projectId)
        {
            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects/" + projectId);

            Project test = result.GetData<Project>();

            Console.WriteLine(test.Members[0].Username);

            return result.GetData<Project>();
        }

        /// <summary>
        /// Removes target <see cref="User"/> using <paramref name="userId"/> of the target <see cref="Project"/> using <paramref name="projectId"/>.
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <para></para>
        /// <returns>Returns <see langword="true"/> if successful or <see langword="false"/> if unsuccessful.</returns>
        /// </summary>
        public async Task<bool> RemoveMember(int projectId, int userId)
        {
            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects/" + projectId + "/" + userId + "/remove");

            return result.GetData<bool>();
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
            ApiResult result = await _httpClient.PutJsonAsync<ApiResult>("api/projects/" + projectId + "/" + userId, new
            {
                userId,
                projectId
            });

            return result.GetData<bool>();
        }
    }
}

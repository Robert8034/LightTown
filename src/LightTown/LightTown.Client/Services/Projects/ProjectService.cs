using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LightTown.Core;
using LightTown.Core.Models.Projects;

namespace LightTown.Client.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;

        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Project>> GetProjects()
        {

            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects");

            return result.GetData<List<Project>>();

        }

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

        public async Task<Project> GetProject(int projectId)
        {
            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects/" + projectId);

            return result.GetData<Project>();
        }

        public async Task<bool> RemoveMember(int projectId, int userId)
        {
            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/projects/" + projectId + "/" + userId + "/remove");

            return result.GetData<bool>();
        }

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

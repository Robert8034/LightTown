using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
            return await _httpClient.GetJsonAsync<List<Project>>("api/projects");
        }

        public async Task<bool> CreateProject(string projectName, string description)
        {

            return await _httpClient.PostJsonAsync<bool>("api/projects", new
            {
                projectName,
                description
            }
            );
        }
    }
}

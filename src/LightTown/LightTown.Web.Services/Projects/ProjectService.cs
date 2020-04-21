using LightTown.Core;
using LightTown.Core.Models.Projects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LightTown.Web.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;

        public ProjectService(HttpClient httpClient)
        {

        }

        public async Task<List<Project>> GetProjects()
        {
            return await _httpClient.PostJsonAsync<List<Project>>("api/projects", null); 
            
        }
    }
}

﻿using System.Collections.Generic;
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

        public async Task<bool> CreateProject(string projectName, string projectDescription)
        {
            ApiResult result = await _httpClient.PostJsonAsync<ApiResult>("api/projects", new
                {
                    projectName,
                    projectDescription
                }
            );

            return result.GetData<bool>();
        }
    }
}

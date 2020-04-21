using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Domain.Projects;
using LightTown.Core.Domain.Roles;
using LightTown.Server.Services.Projects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LightTown.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Route("/{projectId}/members")]
        public async Task<ApiResult> GetMembers(int projectId)
        {
            return ApiResult.NoContent();
        }

        [HttpPut]
        [Route("/{projectId}/members/{userId}")]
        public async Task<ApiResult> PutMember(int projectId, int userId)
        {
            return ApiResult.NoContent();
        }

        [HttpGet]
        [Route("")]
        [Authorization(Permissions.VIEW_ALL_PROJECTS)]
        public async Task<ApiResult> GetProjects()
        {
            List<Project> projects = await _projectService.GetProjects();

            return ApiResult.Success(null);
        }

        [HttpGet]
        [Route("{projectId}")]
        public async Task<ApiResult> GetProject(int projectId)
        {
            return ApiResult.NoContent();
        }

        [HttpPost]
        [Route("")]
        public async Task<ApiResult> PostProject()
        {
            return ApiResult.NoContent();
        }
    }
}

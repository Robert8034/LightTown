using System.Collections.Generic;
using System.Threading.Tasks;
using LightTown.Core.Domain.Projects;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Users;
using LightTown.Server.Services.Projects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LightTown.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _projectService = projectService;
        }

        [HttpGet]
        [Route("/{projectId}/members")]
        public async Task<ApiResult> GetMembers(int projectId)
        {
            return ApiResult.NoContent();
        }

        [HttpPut]
        [Route("/{projectId}/members")]
        public async Task<ApiResult> PutMember(int projectId)
        {
            return ApiResult.NoContent();
        }

        [HttpGet]
        [Route("")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> GetProjects()
        {
            //TODO: add mapping + remove hardcoded project when permissions works.
            //List<Project> projects = await _projectService.GetProjects();
            List<Project> projects = new List<Project>();
            Project project = new Project();
            project.ProjectName = "test";
            projects.Add(project);
            return ApiResult.Success(projects);
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

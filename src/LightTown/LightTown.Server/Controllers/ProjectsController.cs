using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Data;
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
        private readonly IMapper _mapper;

        public ProjectsController(IProjectService projectService, UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _projectService = projectService;
            _mapper = mapper;
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
            List<Project> projects = await _projectService.GetProjects();

            var projectsModel = _mapper.Map<List<Core.Models.Projects.Project>>(projects);

            return ApiResult.Success(projectsModel);
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

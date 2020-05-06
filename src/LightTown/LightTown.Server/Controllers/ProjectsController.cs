using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Data;
using LightTown.Core.Domain.Projects;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Users;
using LightTown.Server.Models.Projects;
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
        public ApiResult GetProjects()
        {
            List<Project> projects = _projectService.GetProjects();
            
            var projectModels = _mapper.Map<List<Core.Models.Projects.Project>>(projects);

            return ApiResult.Success(projectModels);
        }

        [HttpGet]
        [Route("{projectId}")]
        [Authorization(Permissions.NONE)]
        public ApiResult GetProject(int projectId)
        {
            Project project = _projectService.GetProject(projectId);

            if(project == null)
                return ApiResult.BadRequest();

            var projectModel = _mapper.Map<Core.Models.Projects.Project>(project);

            return ApiResult.Success(projectModel);
        }

        [HttpPost]
        [Route("")]
        [Authorization(Permissions.CREATE_PROJECTS)]
        public async Task<ApiResult> CreateProject([FromBody] ProjectPost projectPost)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var project = _projectService.CreateProject(projectPost.ProjectName, projectPost.ProjectDescription, currentUser.Id);

            var projectModel = _mapper.Map<Core.Models.Projects.Project>(project);

            return ApiResult.Success(projectModel);
        }
    }
}

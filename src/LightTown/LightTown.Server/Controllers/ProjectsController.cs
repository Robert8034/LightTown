using System;
using System.Collections.Generic;
using System.Linq;
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
        [Route("/{projectId}/members/{userId}")]
        [Authorization(Permissions.MANAGE_PROJECTS)]
        public ApiResult PutMember(int projectId, int userId)
        {
            //TODO: add checks for existence of project and user

            var result = _projectService.AddMember(projectId, userId);

            return result ? ApiResult.NoContent() : ApiResult.BadRequest();
        }

        [HttpGet]
        [Route("/{projectId}/{userId}/remove")]
        public ApiResult RemoveMember(int projectId, int userId)
        {
            bool result = false;

            var project = _projectService.GetProject(projectId);

            var member = project.ProjectMembers.Find(e => e.MemberId == userId);

            if (member != null)
            {
                project.ProjectMembers.Remove(member);
                result = _projectService.PutProject(project);
            }

            return result ? ApiResult.Success(result) : ApiResult.BadRequest();
        }

        [HttpGet]
        [Route("")]
        [Authorization(Permissions.VIEW_ALL_PROJECTS)]
        public ApiResult GetProjects()
        {
            var projects = _projectService.GetProjectsWithTagIdsAndMemberCount();

            var projectModels = new List<Core.Models.Projects.Project>();

            foreach (var project in projects)
            {
                var projectModel = _mapper.Map<Core.Models.Projects.Project>(project.Item1);
                projectModel.MemberCount = project.Item2;
                projectModel.Tags = project.Item3.ToList();
                projectModels.Add(projectModel);
            }

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
            projectModel.Members = new List<Core.Models.Users.User>(); //TODO: add cache functions so this isn't needed anymore

            foreach (var projectMember in project.ProjectMembers)
            {
                projectModel.Members.Add(_mapper.Map<Core.Models.Users.User>(projectMember.Member));
            }

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

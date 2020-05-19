using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LightTown.Core;
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
        private readonly IProjectMemberService _projectMemberService;
        private readonly RoleManager<Role> _roleManager;

        public ProjectsController(IProjectService projectService, UserManager<User> userManager, IMapper mapper, IProjectMemberService projectMemberService, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _projectService = projectService;
            _mapper = mapper;
            _projectMemberService = projectMemberService;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Get a list of members (User objects) of a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <response code="200">Valid response with the list of members.</response>
        /// <response code="400">Project doesn't exist.</response>
        /// <response code="401">The user isn't authorized.</response>
        [HttpGet]
        [Route("/{projectId}/members")]
        [Authorization(Permissions.NONE)]
        public ApiResult GetMembers(int projectId)
        {
            bool projectExists = _projectService.ProjectExists(projectId);

            if (!projectExists)
                return ApiResult.BadRequest();

            var members = _projectService.GetMembers(projectId);

            var memberModels = _mapper.Map<List<Core.Models.Users.User>>(members);

            return ApiResult.Success(memberModels);
        }

        /// <summary>
        /// Add a user to a project.
        /// </summary>
        /// <param name="projectId">The project id of the project to add the user to.</param>
        /// <param name="userId">The user id of the user to add.</param>
        /// <response code="204">User has been added to the project.</response>
        /// <response code="400">Project or user doesn't exist.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="403">The user is authorized but doesn't have permission to this endpoint or to manage this project.</response>
        [HttpPut]
        [Route("/{projectId}/members/{userId}")]
        [Authorization(Permissions.MANAGE_PROJECTS)]
        public async Task<ApiResult> AddMember(int projectId, int userId)
        {
            bool projectExists = _projectService.ProjectExists(projectId);

            if(!projectExists)
                return ApiResult.BadRequest();

            //HACK: this works but is bad for performance since it gets the entire user object just to check if it exists
            bool userExists = await _userManager.FindByIdAsync(userId.ToString()) != null;

            if (!userExists)
                return ApiResult.BadRequest();

            _projectMemberService.CreateProjectMember(projectId, userId);

            return ApiResult.NoContent();
        }

        /// <summary>
        /// Remove a member from a project.
        /// </summary>
        /// <param name="projectId">The project id of the project to remove the member from.</param>
        /// <param name="memberId">The user id of the member to remove.</param>
        /// <response code="204">User has been removed from the project.</response>
        /// <response code="400">Project or user doesn't exist or the user is not a member of the project.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="403">The user is authorized but doesn't have permission to this endpoint or to manage this project.</response>
        [HttpDelete]
        [Route("/{projectId}/members/{memberId}")]
        [Authorization(Permissions.MANAGE_PROJECTS)]
        public ApiResult RemoveMember(int projectId, int memberId)
        {
            var projectMember = _projectMemberService.GetProjectMember(projectId, memberId);

            if (projectMember == null)
                return ApiResult.BadRequest();

            _projectMemberService.RemoveProjectMember(projectMember);

            return ApiResult.NoContent();
        }

        /// <summary>
        /// Get a list of projects that the user has view access to that include their member count and tag ids.
        /// </summary>
        /// <response code="200">Valid response with the list of projects.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="403">The user is authorized but doesn't have permission to this endpoint.</response>
        [HttpGet]
        [Route("")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> GetProjects()
        {
            bool hasAccessToAllProjects = true;

            //get the roles of the current user.
            var roles = (await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User)))
                .Select(async roleName => await _roleManager.FindByNameAsync(roleName))
                .Select(e => e.Result);

            //if none of the roles has the VIEW_ALL_PROJECTS permission we will only return the projects the user has access to.
            if (!roles.Any(role => role.Permissions.HasFlag(Permissions.VIEW_ALL_PROJECTS)))
                hasAccessToAllProjects = false;

            var projects = _projectService
                .GetProjectsWithTagIdsAndMemberCount(hasAccessToAllProjects ? (int?) null : int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

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

        /// <summary>
        /// Get a project that the user has view access to.
        /// </summary>
        /// <response code="200">Valid response with the project.</response>
        /// <response code="400">Project doesn't exist.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="403">The user is authorized but doesn't have the <see cref="Permissions.VIEW_ALL_PROJECTS"/> permission and isn't a member of this project.</response>
        [HttpGet]
        [Route("{projectId}")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> GetProject(int projectId)
        {
            //get the roles of the current user.
            var roles = (await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User)))
                .Select(async roleName => await _roleManager.FindByNameAsync(roleName))
                .Select(e => e.Result);

            //if none of the roles has the VIEW_ALL_PROJECTS permission and the current user is not a member of the project we return a 403 forbidden result.
            if (!roles.Any(role => role.Permissions.HasFlag(Permissions.VIEW_ALL_PROJECTS)))
            {
                if (!_projectService.UserIsMember(projectId, int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))))
                    return ApiResult.Forbidden("You do not have access to this project.");
            }

            Project project = _projectService.GetProject(projectId);

            if (project == null)
                return ApiResult.BadRequest();

            var projectModel = _mapper.Map<Core.Models.Projects.Project>(project);

            return ApiResult.Success(projectModel);
        }

        /// <summary>
        /// Create a project and return the created project.
        /// </summary>
        /// <response code="200">Valid response with the created project.</response>
        /// <response code="400">Invalid request data given.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="403">The user is authorized but doesn't have permission to this endpoint.</response>
        [HttpPost]
        [Route("")]
        [Authorization(Permissions.CREATE_PROJECTS)]
        public async Task<ApiResult> CreateProject([FromBody] ProjectPost projectPost)
        {
            if (!ModelState.IsValid)
                return ApiResult.BadRequest(ModelState.First(e => e.Value.Errors.Any()).Value.Errors.First().ErrorMessage);

            var currentUser = await _userManager.GetUserAsync(User);

            var project = _projectService.CreateProject(projectPost.ProjectName, projectPost.ProjectDescription, currentUser.Id);

            var projectModel = _mapper.Map<Core.Models.Projects.Project>(project);

            return ApiResult.Success(projectModel);
        }
    }
}

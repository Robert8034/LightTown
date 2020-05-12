using System;
using LightTown.Core.Domain.Projects;
using System.Collections.Generic;
using System.Linq;
using LightTown.Core.Data;
using LightTown.Core.Domain.Users;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LightTown.Server.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<ProjectMember> _projectMemberRepository;

        public ProjectService(IRepository<Project> projectRepository, UserManager<User> userManager, IRepository<ProjectMember> projectMemberRepository)
        {
            _projectRepository = projectRepository;
            _userManager = userManager;
            _projectMemberRepository = projectMemberRepository;
        }

        /// <summary>
        /// Get all projects.
        /// </summary>
        /// <returns>A list of all projects.</returns>
        public List<Project> GetProjects()
        {
            var projects = _projectRepository.TableNoTracking.ToList();

            return projects;
        }

        /// <summary>
        /// Get all projects with their member count.
        /// </summary>
        /// <returns>A list of all projects with their member count.</returns>
        public List<(Project, int)> GetProjectsWithMemberCount()
        {
            var projects = _projectRepository.TableNoTracking.Select(e =>
                new Tuple<Project, int>(e, e.ProjectMembers.Count(e2 => e2.ProjectId == e.Id)).ToValueTuple()).ToList();

            return projects;
        }

        /// <summary>
        /// Create a project and add the creator as a member of the project.
        /// <para>
        /// The user is 
        /// </para>
        /// </summary>
        /// <param name="projectName">Name of the project, required.</param>
        /// <param name="projectDescription">Description of the project, optional.</param>
        /// <param name="creatorId">The creator of the project's user id, required. The creator will also become a member of the project.</param>
        /// <returns>Returns the created project.</returns>
        public Project CreateProject(string projectName, string projectDescription, int creatorId)
        {
            Project project = new Project
            {
                ProjectName = projectName,
                ProjectDescription = projectDescription ?? "",
                CreationDateTime = DateTime.Now,
                CreatorId = creatorId,
                ProjectMembers = new List<ProjectMember>(new []
                {
                    new ProjectMember
                    {
                        MemberId = creatorId
                    }
                })
            };

            return _projectRepository.Insert(project);
        }

        /// <summary>
        /// Get a project based on the project id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The project, <see langword="null"/> if no project with the id exists.</returns>
        public Project GetProject(int projectId)
        {
            return _projectRepository.TableNoTracking.SingleOrDefault(e => e.Id == projectId);
        }

        /// <summary>
        /// Adds a member to a project based on the target user's ID and project ID
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <para>
        /// <returns>If successful in adding, this method will return <see langword="true"></see>, if not it will return <see langword="false"></see>. </returns>
        /// </para>
        /// </summary>
        public bool AddMember(int projectId, int userId)
        {
            if (_projectMemberRepository.TableNoTracking.Any(e => e.ProjectId == projectId && e.MemberId == userId))
                return false;

            _projectMemberRepository.Insert(new ProjectMember
            {
                ProjectId = projectId,
                MemberId = userId
            });

            return true;
        }

        public bool PutProject(Project project)
        {
            _projectRepository.Update(project);

            return true;
        }
    }
}

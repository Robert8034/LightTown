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
        public IEnumerable<Project> GetProjects()
        {
            var projects = _projectRepository.TableNoTracking.ToList();

            return projects;
        }

        /// <summary>
        /// Get all projects with their member count and list of tag ids.
        /// </summary>
        /// <returns>A list of all projects with their member count.</returns>
        public IEnumerable<(Project, int, IEnumerable<int>)> GetProjectsWithTagIdsAndMemberCount()
        {
            var projects = _projectRepository.TableNoTracking.Select(project =>
                new Tuple<Project, int, IEnumerable<int>>(project, 
                    project.ProjectMembers.Count(projectMember => projectMember.ProjectId == project.Id), 
                    project.ProjectTags.Select(projectTag => projectTag.TagId)).ToValueTuple()).ToList();

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
            var project = _projectRepository.TableNoTracking.SingleOrDefault(e => e.Id == projectId);

            return project;
        }

        /// <summary>
        /// Get a list of ProjectMember objects for a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<ProjectMember> GetProjectMembers(int projectId)
        {
            return _projectMemberRepository.Table.Where(e => e.ProjectId == projectId).Include(e => e.Member).ToList();
        }

        /// <summary>
        /// Add a user to a project.
        /// <para>
        /// The <paramref name="projectId"/> and <paramref name="userId"/> parameters are expected to be valid.
        /// </para>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// </summary>
        /// <returns>If successful in adding, this method will return <see langword="true"></see>, if not it will return <see langword="false"></see>. </returns>
        public void AddMember(int projectId, int userId)
        {
            _projectMemberRepository.Insert(new ProjectMember
            {
                ProjectId = projectId,
                MemberId = userId
            });
        }

        /// <summary>
        /// Updates a project
        /// <param name="project"></param>
        /// <para>
        /// <returns>Returns <see langword="true"></see></returns>
        /// </para>
        /// </summary>

        public bool PutProject(Project project)
        {
            _projectRepository.Update(project);

            return true;
        }

        /// <summary>
        /// Check whether a project with the project id exists.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Returns <see langword="true"></see> if a project exists, <see langword="false"></see> otherwise.</returns>
        public bool ProjectExists(int projectId)
        {
            return _projectRepository.Table.Any(e => e.Id == projectId);
        }

        /// <summary>
        /// Get a list of members (User objects) of a certain project, assuming <paramref name="projectId"/> is a valid project id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<User> GetMembers(int projectId)
        {
            return _projectMemberRepository.TableNoTracking
                .Where(projectMember => projectMember.ProjectId == projectId)
                .Select(projectMember => projectMember.Member)
                .ToList();
        }
    }
}

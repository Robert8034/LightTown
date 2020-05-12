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
        public ProjectService(IRepository<Project> projectRepository, UserManager<User> userManager)
        {
            _projectRepository = projectRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Get all projects.
        /// </summary>
        /// <returns>A list of all projects.</returns>
        public List<Project> GetProjects()
        {
            return _projectRepository.TableNoTracking.ToList();
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
        /// <returns>If successfull in adding, this method will return <see langword="true"></see>, if not it will return <see langword="false"></see>. </returns>
        /// </para>
        /// </summary>
        public async Task<bool> AddMemberAsync(int projectId, int userId)
        {
            Project project = _projectRepository.TableNoTracking.Include(e => e.ProjectMembers).SingleOrDefault(e => e.Id == projectId);
            if (project.ProjectMembers.Find(e => e.Id == userId) == null)
            {
                User user = await _userManager.FindByIdAsync(userId.ToString());
                project.ProjectMembers.Add(new ProjectMember
                {
                    MemberId = userId
                });
                _projectRepository.Update(project);
                return true;
            }
            return false;
        }

        public bool PutProject(Project project)
        {
            _projectRepository.Update(project);

            return true;
        }
    }
}

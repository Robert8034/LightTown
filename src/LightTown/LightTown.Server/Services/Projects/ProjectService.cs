using System;
using LightTown.Core.Domain.Projects;
using System.Collections.Generic;
using System.Linq;
using LightTown.Core.Data;
using LightTown.Core.Domain.Users;

namespace LightTown.Server.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        public ProjectService(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
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
    }
}

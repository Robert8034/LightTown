using System;
using LightTown.Core.Domain.Projects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Data;
using LightTown.Server.Models.Projects;
using Microsoft.EntityFrameworkCore;

namespace LightTown.Server.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        public ProjectService(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<Project>> GetProjects()
        {
            return _projectRepository.Table.ToList();
        }

        public async Task<bool> PostProject(ProjectPost project)
        {
            Project newProject = new Project
            {
                ProjectName = project.ProjectName,
                ProjectDescription = project.ProjectDescription,
                CreationDateTime = DateTime.Now,
                Members = new List<Core.Domain.Users.User>()
            };

            newProject = _projectRepository.Insert(newProject);

            return newProject.ProjectName.Equals(project.ProjectName);
        }

        public Project GetProject(int projectId)
        {
            return _projectRepository.Table.Include(e => e.Members).SingleOrDefault(e => e.Id == projectId);
        }
    }
}

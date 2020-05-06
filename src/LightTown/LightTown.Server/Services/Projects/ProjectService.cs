using System;
using LightTown.Core.Domain.Projects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Data;
using LightTown.Core.Domain.Users;
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
            return _projectRepository.Table.Include(e => e.Members).ToList();
        }

        public async Task<bool> PostProject(ProjectPost project, User user)
        {
            Project newProject = new Project
            {
                ProjectName = project.ProjectName,
                ProjectDescription = project.ProjectDescription,
                CreationDateTime = DateTime.Now,
                CreatorId = user.Id,
                Members = new List<ProjectMember>()
            };

            newProject.Members.Add(new ProjectMember{
                ProjectId = newProject.Id,
                MemberId = user.Id
            });

            newProject = _projectRepository.Insert(newProject);

            return newProject.ProjectName.Equals(project.ProjectName);
        }

        public Project GetProject(int projectId)
        {
            return _projectRepository.Table.Include(e => e.Members).SingleOrDefault(e => e.Id == projectId);
        }

        public async Task<bool> AddMember(int projectId, int userId)
        {
            return true;
        }
    }
}

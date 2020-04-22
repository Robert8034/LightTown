using LightTown.Core.Domain.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightTown.Core.Data;

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
    }
}

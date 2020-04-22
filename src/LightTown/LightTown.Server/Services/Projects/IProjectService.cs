using System.Collections.Generic;
using System.Threading.Tasks;
using LightTown.Core.Domain.Projects;

namespace LightTown.Server.Services.Projects
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjects();
    }
}

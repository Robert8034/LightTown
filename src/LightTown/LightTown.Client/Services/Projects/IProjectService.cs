using System.Collections.Generic;
using System.Threading.Tasks;
using LightTown.Core.Models.Projects;

namespace LightTown.Client.Services.Projects
{
    public interface IProjectService
    {
        //Task<List<Project>> GetProjects();
        Task<Project> CreateProject(string projectName, string projectDescription);
        Task<bool> RemoveMember(int Id, int userId);
    }
}

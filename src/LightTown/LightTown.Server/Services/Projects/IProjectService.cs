using System.Collections.Generic;
using System.Threading.Tasks;
using LightTown.Core.Domain.Projects;
using LightTown.Core.Domain.Users;

namespace LightTown.Server.Services.Projects
{
    public interface IProjectService
    {
        List<Project> GetProjects();
        Project CreateProject(string projectName, string projectDescription, int creatorId);
        Project GetProject(int projectId);
        List<(Project, int, IEnumerable<int>)> GetProjectsWithTagIdsAndMemberCount();
        bool AddMember(int projectId, int userId);
        bool PutProject(Project project);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using LightTown.Core.Domain.Projects;
using LightTown.Core.Domain.Users;

namespace LightTown.Server.Services.Projects
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjects();
        Project CreateProject(string projectName, string projectDescription, int creatorId);
        Project GetProject(int projectId);
        IEnumerable<(Project, int, IEnumerable<int>)> GetProjectsWithTagIdsAndMemberCount();
        void AddMember(int projectId, int userId);
        bool PutProject(Project project);
        bool ProjectExists(int projectId);
        IEnumerable<User> GetMembers(int projectId);
    }
}

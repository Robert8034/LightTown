using System.Collections.Generic;
using System.Threading.Tasks;
using LightTown.Core.Models.Messages;
using LightTown.Core.Models.Projects;
using LightTown.Core.Models.Users;

namespace LightTown.Client.Services.Projects
{
    public interface IProjectService
    {
        //Task<List<Project>> GetProjects();
        Task<Project> CreateProject(string projectName, string projectDescription);
        Task<bool> RemoveMember(int Id, int userId);
        Task<bool> AddMember(int userId, int projectId);
        Task<List<User>> GetProjectMembers(int projectId);
        Task<List<Project>> SearchProjects(string searchValue);
        Task<bool> PostProjectMessage(int projectId, string content, string title); 
        Task<List<Message>> GetProjectMessages(int projectId);
    }
}

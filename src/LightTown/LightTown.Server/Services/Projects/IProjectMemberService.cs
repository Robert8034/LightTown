using LightTown.Core.Domain.Projects;

namespace LightTown.Server.Services.Projects
{
    public interface IProjectMemberService
    {
        public ProjectMember GetProjectMember(int projectId, int memberId);
        void RemoveProjectMember(ProjectMember projectMember);
    }
}
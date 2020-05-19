using System.Linq;
using LightTown.Core.Data;
using LightTown.Core.Domain.Projects;

namespace LightTown.Server.Services.Projects
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IRepository<ProjectMember> _projectMemberRepository;

        public ProjectMemberService(IRepository<ProjectMember> projectMemberRepository)
        {
            _projectMemberRepository = projectMemberRepository;
        }

        /// <summary>
        /// Get a project member based on the project id and member id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="memberId">The id of the member (user).</param>
        /// <returns>Returns a ProjectMember object or <see langword="null"/> if none is found.</returns>
        public ProjectMember GetProjectMember(int projectId, int memberId)
        {
            return _projectMemberRepository.TableNoTracking.SingleOrDefault(e =>
                e.ProjectId == projectId && e.MemberId == memberId);
        }

        /// <summary>
        /// Remove a project member, assuming the <paramref name="projectMember"/> parameter is a valid entity.
        /// </summary>
        /// <param name="projectMember">The ProjectMember object to remove.</param>
        public void RemoveProjectMember(ProjectMember projectMember)
        {
            _projectMemberRepository.Delete(projectMember);
        }

        public void CreateProjectMember(int projectId, int userId)
        {
            _projectMemberRepository.Insert(new ProjectMember
            {
                ProjectId = projectId,
                MemberId = userId
            });
        }
    }
}
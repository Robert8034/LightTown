using System.Collections.Generic;
using LightTown.Core.Domain.Projects;

namespace LightTown.Server.Services.Projects
{
    public interface IProjectMemberService
    {
        public ProjectMember GetProjectMember(int projectId, int memberId);

        /// <summary>
        /// Removes a user from a project. 
        /// <para>
        /// The <paramref name="projectMember"/> is expected to be not null.
        /// </para>
        /// <param name="projectMember"></param>
        /// </summary>
        void RemoveProjectMember(ProjectMember projectMember);

        /// <summary>
        /// Add a user to a project.
        /// <para>
        /// The <paramref name="projectId"/> and <paramref name="userId"/> parameters are expected to be valid.
        /// </para>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// </summary>
        /// <returns>If successful in adding, this method will return <see langword="true"></see>, if not it will return <see langword="false"></see>. </returns>
        public void CreateProjectMember(int projectId, int userId);

        /// <summary>
        /// Gets a list of project members from targeted <paramref name="projectId"/>.
        /// <para>
        /// The <paramref name="projectId"/> parameter is expected to be valid.
        /// </para>
        /// <param name="projectId"></param>
        /// </summary>
        /// <returns>If there are matches, it will return these in a list. If not it will return a new empty list. </returns>
        List<ProjectMember> GetProjectMembers(int projectId);
    }
}
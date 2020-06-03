using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LightTown.Core.Domain.Projects;
using LightTown.Core.Domain.Tags;
using LightTown.Core.Domain.Users;

namespace LightTown.Server.Services.Projects
{
    public interface IProjectService
    {
        /// <summary>
        /// Get all projects.
        /// </summary>
        /// <returns>A list of all projects.</returns>
        IEnumerable<Project> GetProjects();

        /// <summary>
        /// Get all projects with their member count and list of tag ids, optionally filtered for only projects with a certain member.
        /// </summary>
        /// <param name="userIdFilter">If not null this will only return projects this user has access to.</param>
        /// <returns>A list of all projects with their member count.</returns>
        IEnumerable<(Project, int, IEnumerable<int>)> GetProjectsWithTagIdsAndMemberCount(int? userIdFilter);

        /// <summary>
        /// Create a project and add the creator as a member of the project.
        /// <para>
        /// The user is 
        /// </para>
        /// </summary>
        /// <param name="projectName">Name of the project, required.</param>
        /// <param name="projectDescription">Description of the project, optional.</param>
        /// <param name="creatorId">The creator of the project's user id, required. The creator will also become a member of the project.</param>
        /// <returns>Returns the created project.</returns>
        Project CreateProject(string projectName, string projectDescription, int creatorId);

        /// <summary>
        /// Get a project based on the project id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The project, <see langword="null"/> if no project with the id exists.</returns>
        Project GetProject(int projectId);

        /// <summary>
        /// Get a list of ProjectMember objects for a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IEnumerable<ProjectMember> GetProjectMembers(int projectId);

        /// <summary>
        /// Updates a project
        /// <param name="project"></param>
        /// <para>
        /// <returns>Returns <see langword="true"></see></returns>
        /// </para>
        /// </summary>
        bool PutProject(Project project);

        /// <summary>
        /// Check whether a project with the project id exists.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Returns <see langword="true"></see> if a project exists, <see langword="false"></see> otherwise.</returns>
        bool ProjectExists(int projectId);

        /// <summary>
        /// Get a list of members (User objects) of a certain project, assuming <paramref name="projectId"/> is a valid project id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IEnumerable<User> GetMembers(int projectId);

        /// <summary>
        /// Returns whether the given user is a member of the project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool UserIsMember(int projectId, int userId);

        /// <summary>
        /// Returns a list of <see cref="Project"/> where the name of the project matches <paramref name="searchValue"/>
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Project> SearchProjects(string searchValue);

        Task<bool> TryModifyProjectImage(int projectId, Stream fileStream, long? contentLength, string contentType);
        bool TryGetProjectImage(string imageFilename, out byte[] imageBytes);
        List<Tag> ModifyProjectTags(int projectId, List<Core.Models.Tags.Tag> tags);
    }
}

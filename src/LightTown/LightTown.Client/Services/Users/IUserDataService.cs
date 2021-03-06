﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LightTown.Core.Models.Messages;
using LightTown.Core.Models.Projects;
using LightTown.Core.Models.Roles;
using LightTown.Core.Models.Tags;
using LightTown.Core.Models.Users;

namespace LightTown.Client.Services.Users
{
    public interface IUserDataService
    {
        /// <summary>
        /// Will be invoked whenever the current user data gets loaded and unloaded.
        /// </summary>
        Func<Task> OnUserDataChange { get; set; }

        /// <summary>
        /// Load current user data, expecting a valid authorization cookie to be set.
        /// </summary>
        Task LoadData();

        /// <summary>
        /// Unload all data including the current user.
        /// </summary>
        Task UnloadData();

        /// <summary>
        /// Get the current user object or <see langword="null"/> if no user is loaded.
        /// </summary>
        /// <returns></returns>
        User GetCurrentUser();

        /// <summary>
        /// Get the list of available projects.
        /// </summary>
        /// <returns></returns>
        Task<List<Project>> GetProjects();

        /// <summary>
        /// Get a project, will get it from the server if it doesn't exist in the cache. Returns <see langword="null"/> on error or if it doesn't exist.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<Project> GetProject(int projectId);

        /// <summary>
        /// Get a user, will get it from the server if it doesn't exist in the cache.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> GetUser(int userId);

        /// <summary>
        /// Gets project members, will get project and/or project members from server if not in cache. Returns <see langword="null"/> on error or if the project doesn't exist.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<List<User>> GetProjectMembers(int projectId);

        /// <summary>
        /// Get a tag, will get it from the server if it doesn't exist in the cache. Returns <see langword="null"/> on error or if it doesn't exist.
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns>One specific tag.</returns>
        Task<Tag> GetTag(int tagId);

        /// <summary>
        /// Get a list of project tags. Returns <see langword="null"/> on error or if the project doesn't exist.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A list of project tags.</returns>
        Task<List<Tag>> GetProjectTags(int projectId);

        /// <summary>
        /// Get a list of tags that a user has.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A list of project tags.</returns>
        Task<List<Tag>> GetUserTags(int userId);

        /// <summary>
        /// Set the current user object or <see langword="null"/> if no user is loaded.
        /// </summary>
        /// <returns></returns>
        void SetCurrentUser(User user);

        /// <summary>
        /// Search tags based on the name from the cached tags list. Optionally exclude some tags.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeList"></param>
        /// <returns></returns>
        List<Tag> SearchTags(string name, List<Tag> excludeList = null);

        /// <summary>
        /// Add tags to the cache if they don't already exist. Returns a list of inserted tags.
        /// </summary>
        /// <param name="tags"></param>
        IEnumerable<Tag> SetTags(List<Tag> tags);

        /// <summary>
        /// Get the list of roles.
        /// </summary>
        /// <returns></returns>
        Task<List<Role>> GetRoles();

        Task<List<Message>> GetProjectMessages(int projectId);
    }
}
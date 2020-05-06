﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LightTown.Core.Domain.Projects;
using LightTown.Server.Models.Projects;

namespace LightTown.Server.Services.Projects
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjects();
        Task<bool> PostProject(ProjectPost project);
        Project GetProject(int projectId);
        Task<bool> AddMember(int projectId, int userId);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LightTown.Core.Models.Projects;

namespace LightTown.Web.Services.Projects
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjects();
        Task<bool> CreateProject(string projectName, string description);
    }
}

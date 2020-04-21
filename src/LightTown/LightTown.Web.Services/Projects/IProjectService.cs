using LightTown.Core.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LightTown.Web.Services.Projects
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjects();
    }
}

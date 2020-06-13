using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Domain.Projects;

namespace LightTown.Server.Services.Projects
{
    public interface IProjectLikeService
    {
        void LikeProject(int projectId, int userId);
        void RemoveProjectLike(ProjectLike projectLike);
        int GetProjectLikeCount(int projectId);
        ProjectLike GetProjectLike(int projectId, int userId);
        bool LikeExists(int projectId, int userId);
    }
}

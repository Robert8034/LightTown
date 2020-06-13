using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Data;
using LightTown.Core.Domain.Projects;

namespace LightTown.Server.Services.Projects
{
    public class ProjectLikeService : IProjectLikeService
    {
        private readonly IRepository<ProjectLike> _projectLikeRepository;
        public ProjectLikeService(IRepository<ProjectLike> projectLikeRepository)
        {
            _projectLikeRepository = projectLikeRepository;
        }

        public int GetProjectLikeCount(int projectId)
        {
            return _projectLikeRepository.TableNoTracking.Count(e => e.ProjectId == projectId);
        }

        public void LikeProject(int projectId, int userId)
        {
            _projectLikeRepository.Insert(new ProjectLike
            {
                ProjectId = projectId,
                UserId = userId
            });
        }

        public void RemoveProjectLike(ProjectLike projectLike)
        {
            _projectLikeRepository.Delete(projectLike);
        }

        public ProjectLike GetProjectLike(int projectId, int userId)
        {
            return _projectLikeRepository.TableNoTracking.SingleOrDefault(e =>
                e.ProjectId == projectId && e.UserId == userId);
        }

        public bool LikeExists(int projectId, int userId)
        {
            return _projectLikeRepository.Table.Any(e => e.ProjectId == projectId && e.UserId == userId);
        }
    }
}

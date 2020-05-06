using AutoMapper;
using LightTown.Core.Models.Users;

namespace LightTown.Core.Models.Projects
{
    [AutoMap(typeof(Domain.Projects.ProjectMember))]
    public class ProjectMember
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int MemberId { get; set; }
        public User Member { get; set; }
    }
}
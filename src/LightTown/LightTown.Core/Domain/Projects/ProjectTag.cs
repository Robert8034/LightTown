using LightTown.Core.Data;
using LightTown.Core.Domain.Tags;

namespace LightTown.Core.Domain.Projects
{
    public class ProjectTag : BaseEntity
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}

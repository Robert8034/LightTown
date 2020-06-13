using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Data;

namespace LightTown.Core.Domain.Projects
{
    public class ProjectLike : BaseEntity
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}

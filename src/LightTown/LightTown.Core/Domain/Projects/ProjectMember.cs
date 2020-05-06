using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Data;
using LightTown.Core.Domain.Users;

namespace LightTown.Core.Domain.Projects
{
    public class ProjectMember : BaseEntity
    {
        public int ProjectId { get; set; }
        public int MemberId { get; set; }
        public User Member { get; set; }
    }
}

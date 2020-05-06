using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Data;
using LightTown.Core.Domain.Users;

namespace LightTown.Core.Domain.Projects
{
    public class Project : BaseEntity
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        //public List<Post> ProjectPosts { get; set; }
        public List<ProjectMember> Members { get; set; }
        public int CreatorId { get; set; }
        //public List<Tag> Tags { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using AutoMapper;
using LightTown.Core.Models.Users;

namespace LightTown.Core.Models.Projects
{
    [AutoMap(typeof(Domain.Projects.Project))]
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        //public List<Post> ProjectPosts { get; set; }
        public List<User> Members { get; set; }
        public List<int> Tags { get; set; }
        public int? MemberCount { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
    }
}
 
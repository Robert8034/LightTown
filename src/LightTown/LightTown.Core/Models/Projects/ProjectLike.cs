using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace LightTown.Core.Models.Projects
{
    [AutoMap(typeof(Domain.Projects.ProjectLike))]
    public class ProjectLike
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}

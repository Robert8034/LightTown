using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.Projects
{
    public class ProjectLikeMap : EntityMappingConfiguration<ProjectLike>
    {
        public override void Configure(EntityTypeBuilder<ProjectLike> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}

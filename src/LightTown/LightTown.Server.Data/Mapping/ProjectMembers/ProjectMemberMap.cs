using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.ProjectMembers
{
    public class ProjectMemberMap : EntityMappingConfiguration<ProjectMember>
    {
        public override void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Member);
        }
    }
}

using LightTown.Core.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.Projects
{
    public class ProjectMemberMap : EntityMappingConfiguration<ProjectMember>
    {
        public override void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Member);
            builder.HasOne(e => e.Project)
                .WithMany(e => e.ProjectMembers);
        }
    }
}

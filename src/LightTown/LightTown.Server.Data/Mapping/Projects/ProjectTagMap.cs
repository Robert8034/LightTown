using LightTown.Core.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.Projects
{
    public class ProjectTagMap : EntityMappingConfiguration<ProjectTag>
    {
        public override void Configure(EntityTypeBuilder<ProjectTag> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Tag);
            builder.HasOne(e => e.Project)
                .WithMany(e => e.ProjectTags);
        }
    }
}
using LightTown.Core.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.Projects
{
    public class ProjectMap : EntityMappingConfiguration<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasMany(e => e.ProjectMembers);
            builder.HasMany(e => e.ProjectTags);
            builder.HasMany(e => e.ProjectLikes);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Domain.Tags;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.Tags
{
    public class TagMap : EntityMappingConfiguration<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}

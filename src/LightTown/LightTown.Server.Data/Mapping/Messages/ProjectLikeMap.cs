using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Domain.Messages;
using LightTown.Core.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.Messages
{
    public class MessageLikeMap : EntityMappingConfiguration<MessageLike>
    {
        public override void Configure(EntityTypeBuilder<MessageLike> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Domain.Messages;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.Messages
{
    public class MessageMap : EntityMappingConfiguration<Message>
    {
        public override void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasMany(e => e.MessageLikes);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.Users
{
    class UserTagMap : EntityMappingConfiguration<UserTag>
    {
        public override void Configure(EntityTypeBuilder<UserTag> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Tag);
            builder.HasOne(e => e.User)
                .WithMany(e => e.Tags);
        }
    }
}

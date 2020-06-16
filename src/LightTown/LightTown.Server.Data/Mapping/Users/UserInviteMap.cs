using LightTown.Core.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.Users
{
    public class UserInviteMap : EntityMappingConfiguration<UserInvite>
    {
        public override void Configure(EntityTypeBuilder<UserInvite> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Username).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.RoleId).IsRequired();
            builder.Property(e => e.InviteCode).IsRequired();
            builder.Property(e => e.CreationTime).IsRequired();
        }
    }
}
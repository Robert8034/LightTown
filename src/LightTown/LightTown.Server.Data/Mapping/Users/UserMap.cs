using LightTown.Core.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightTown.Server.Data.Mapping.Users
{
    public class UserMap : EntityMappingConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
           
        }
    }
}

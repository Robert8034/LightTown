using Microsoft.EntityFrameworkCore;

namespace LightTown.Server.Data.Mapping
{
    public interface IEntityMappingConfiguration
    {
        void ApplyConfiguration(ModelBuilder modelBuilder);
    }
}

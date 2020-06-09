using AutoMapper;
using LightTown.Core.Domain.Roles;

namespace LightTown.Core.Models.Roles
{
    [AutoMap(typeof(Domain.Roles.Role))]
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public bool CanBeModified { get; set; }
        public Permissions Permissions { get; set; }
    }
}
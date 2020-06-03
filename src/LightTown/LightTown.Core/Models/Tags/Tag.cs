using AutoMapper;

namespace LightTown.Core.Models.Tags
{
    [AutoMap(typeof(Domain.Tags.Tag))]
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
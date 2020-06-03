using System.Collections.Generic;
using LightTown.Core.Domain.Projects;
using LightTown.Core.Domain.Tags;

namespace LightTown.Server.Services.Tags
{
    public interface ITagService
    {
        IEnumerable<Tag> GetTags();
        
        Tag GetTag(int tagId);

        Tag InsertTag(Core.Models.Tags.Tag tag);
    }
}

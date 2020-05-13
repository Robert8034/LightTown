using System.Collections.Generic;
using System.Linq;
using LightTown.Core.Data;
using LightTown.Core.Domain.Tags;

namespace LightTown.Server.Services.Tags
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagRepository;

        public TagService(IRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        /// <summary>
        /// Get all tags.
        /// </summary>
        /// <returns>A list of all tags.</returns>
        public IEnumerable<Tag> GetTags()
        {
            var tags = _tagRepository.TableNoTracking.ToList();

            return tags;
        }
    }
}

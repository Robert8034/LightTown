using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Domain.Roles;
using LightTown.Server.Services.Tags;
using Microsoft.AspNetCore.Mvc;
using Tag = LightTown.Core.Domain.Tags.Tag;

namespace LightTown.Server.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public TagsController(IMapper mapper, ITagService tagService)
        {
            _mapper = mapper;
            _tagService = tagService;
        }

        /// <summary>
        /// Get a list of tags.
        /// </summary>
        /// <response code="200">Valid response with the list of tags.</response>
        /// <response code="401">The user isn't authorized.</response>
        [HttpGet]
        [Authorization(Permissions.NONE)]
        public ApiResult GetTags()
        {
            IEnumerable<Tag> tags = _tagService.GetTags();

            var tagModels = _mapper.Map<List<Core.Models.Tags.Tag>>(tags);

            return ApiResult.Success(tagModels);
        }

        /// <summary>
        /// Get a tag.
        /// </summary>
        /// <response code="200">Valid response with the tag.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="404">The tag doesn't exist.</response>
        [HttpGet]
        [Route("{tagId}")]
        [Authorization(Permissions.NONE)]
        public ApiResult GetTag(int tagId)
        {
            var tag = _tagService.GetTag(tagId);

            if(tag == null)
                return ApiResult.NotFound();

            var tagModel = _mapper.Map<Core.Models.Tags.Tag>(tag);

            return ApiResult.Success(tagModel);
        }
    }
}

using System.Collections.Generic;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Domain.Roles;
using LightTown.Server.Services.Tags;
using Microsoft.AspNetCore.Mvc;
using Tag = LightTown.Core.Domain.Tags.Tag;

namespace LightTown.Server.Controllers
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

            var tagModels = _mapper.Map<Core.Models.Tags.Tag>(tags);

            return ApiResult.Success(tagModels);
        }

        [HttpGet]
        [Route("{tagId}")]
        [Authorization(Permissions.VIEW_ALL_PROJECTS)]
        public ApiResult GetTag(int tagId)
        {
            var tag = _tagService.GetTag(tagId);

            var tagModel = _mapper.Map<Core.Models.Tags.Tag>(tag);

            return ApiResult.Success(tagModel);
        }
    }
}

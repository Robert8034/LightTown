using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Models.Tags;
using LightTown.Server.Services.Tags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [HttpGet]
        [Authorization(Permissions.NONE)]
        public ApiResult GetTags()
        {
            var tags = _tagService.GetTags();

            var tagModels = _mapper.Map<Core.Models.Tags.Tag>(tags);

            return ApiResult.Success(tagModels);
        }
    }
}

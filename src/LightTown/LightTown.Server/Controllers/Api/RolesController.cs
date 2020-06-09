using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LightTown.Server.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;

        public RolesController(IMapper mapper, RoleManager<Role> roleManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Get a list of roles.
        /// </summary>
        /// <response code="200">Valid response with the list of roles.</response>
        /// <response code="401">The user isn't authorized.</response>
        [HttpGet]
        [Route("")]
        [Authorization(Permissions.NONE)]
        public ApiResult GetRoles()
        {
            IEnumerable<Role> roles = _roleManager.Roles.ToList();

            var roleModels = _mapper.Map<List<Core.Models.Roles.Role>>(roles);

            return ApiResult.Success(roleModels);
        }
    }
}

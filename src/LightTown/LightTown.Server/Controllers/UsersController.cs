using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Users;
using LightTown.Server.Services.Users;
using Microsoft.AspNetCore.Identity;

namespace LightTown.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the current user.
        /// </summary>
        /// <response code="200">Valid response with a user object.</response>
        /// <response code="401">The user isn't authorized.</response>
        [HttpGet]
        [Route("@me")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> GetSelf()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var userModel = _mapper.Map<Core.Models.Users.User>(currentUser);

            return ApiResult.Success(userModel);
        }

        [HttpGet]
        [Route("search/{searchValue}")]
        [Authorization(Permissions.NONE)]
        public ApiResult SearchUsers(string searchValue)
        {
            List<User> users = _userManager.Users.Where(e => e.UserName == searchValue).ToList();

            var usersModel = _mapper.Map<List<Core.Models.Users.User>>(users);

            return ApiResult.Success(usersModel);
        }
    }
}

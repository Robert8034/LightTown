using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Users;
using LightTown.Core.Models.Tags;
using LightTown.Server.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LightTown.Server.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserService _userService;

        public UsersController(UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager, IUserService userService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _userService = userService;
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

            var tagIds = _userService.GetUserTagIds(currentUser.Id);

            var userModel = _mapper.Map<Core.Models.Users.User>(currentUser);
            userModel.TagIds = tagIds;

            return ApiResult.Success(userModel);
        }

        /// <summary>
        /// Modify the current user.
        /// </summary>
        /// <response code="200">Valid response with the updated user object.</response>
        /// <response code="401">The user isn't authorized.</response>
        [HttpPatch]
        [Route("@me")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> ModifySelf(Core.Models.Users.User user)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser.Id != user.Id)
            {
                return ApiResult.Forbidden("You do not have access to modify a different user using this endpoint.");
            }

            var oldUser = await _userManager.FindByIdAsync(user.Id.ToString());

            if (oldUser == null)
                return ApiResult.BadRequest();

            if (_userService.TryModifyUser(user, oldUser, out User newUser))
            {
                var userModel = _mapper.Map<Core.Models.Users.User>(newUser);

                return ApiResult.Success(userModel);
            }

            return ApiResult.BadRequest();
        }


        /// <summary>
        /// Modify a user.
        /// </summary>
        /// <response code="200">Valid response with the updated user object.</response>
        /// <response code="400">The user doesn't exist or has invalid data.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="403">The user doesn't have access to modify this user.</response>
        [HttpPatch]
        [Authorization(Permissions.MANAGE_USERS)]
        public async Task<ApiResult> ModifyUser(Core.Models.Users.User user)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            //get the roles of the current user.
            var roles = (await _userManager.GetRolesAsync(currentUser))
                .Select(async roleName => await _roleManager.FindByNameAsync(roleName))
                .Select(e => e.Result);

            //if none of the roles has the MANAGE_USERS permission we send a forbidden result.
            if (!roles.Any(role => role.Permissions.HasFlag(Permissions.MANAGE_USERS)))
                return ApiResult.Forbidden("You do not have access to modify this user.");

            var oldUser = await _userManager.FindByIdAsync(user.Id.ToString());

            if(oldUser == null)
                return ApiResult.BadRequest();

            if (_userService.TryModifyUser(user, oldUser, out User newUser))
            {
                var userModel = _mapper.Map<Core.Models.Users.User>(newUser);

                return ApiResult.Success(userModel);
            }

            return ApiResult.BadRequest();
        }

        /// <summary>
        /// Modify the current user's avatar.
        /// </summary>
        /// <response code="204">Avatar is updated.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">The user isn't authorized.</response>
        [HttpPut]
        [Route("@me/avatar")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> ModifySelfAvatar()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (await _userService.TryModifyUserAvatar(currentUser, Request.Body, Request.ContentLength, Request.ContentType))
            {
                return ApiResult.NoContent();
            }

            return ApiResult.BadRequest();
        }


        /// <summary>
        /// Modify a user's avatar.
        /// </summary>
        /// <response code="204">Avatar is updated.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="403">The user doesn't have access to modify this user's avatar.</response>
        [HttpPut]
        [Route("{userId}/avatar")]
        [Authorization(Permissions.MANAGE_USERS)]
        public async Task<ApiResult> ModifyUserAvatar(int userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            //get the roles of the current user.
            var roles = (await _userManager.GetRolesAsync(currentUser))
                .Select(async roleName => await _roleManager.FindByNameAsync(roleName))
                .Select(e => e.Result);

            //if none of the roles has the MANAGE_USERS permission we send a forbidden result.
            if (!roles.Any(role => role.Permissions.HasFlag(Permissions.MANAGE_USERS)))
                return ApiResult.Forbidden("You do not have access to modify this user's avatar.");

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return ApiResult.BadRequest();

            if (await _userService.TryModifyUserAvatar(user, Request.Body, Request.ContentLength, Request.ContentType))
            {
                return ApiResult.NoContent();
            }

            return ApiResult.BadRequest();
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

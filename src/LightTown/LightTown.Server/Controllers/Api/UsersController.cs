using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Tags;
using LightTown.Core.Domain.Users;
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
        private readonly IUserService _userService;

        public UsersController(UserManager<User> userManager, IMapper mapper, IUserService userService)
        {
            _userManager = userManager;
            _mapper = mapper;
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
        /// Get the a user.
        /// </summary>
        /// <response code="200">Valid response with a user object.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="404">No user found with the user id.</response>
        [HttpGet]
        [Route("{userId}")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> GetUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if(user == null)
                return ApiResult.NotFound();

            var tagIds = _userService.GetUserTagIds(user.Id);

            var userModel = _mapper.Map<Core.Models.Users.User>(user);
            userModel.TagIds = tagIds;

            return ApiResult.Success(userModel);
        }

        /// <summary>
        /// Modify the current user.
        /// </summary>
        /// <response code="200">Valid response with the updated user object.</response>
        /// <response code="400">Invalid request data given.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="403">The user is trying to modify a different user (wrong endpoint!).</response>
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
        /// Modify a (different) user.
        /// </summary>
        /// <response code="200">Valid response with the updated user object.</response>
        /// <response code="400">The user doesn't exist or has invalid data.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="403">The user doesn't have the right permissions.</response>
        [HttpPatch]
        [Authorization(Permissions.MANAGE_USERS)]
        public async Task<ApiResult> ModifyUser(Core.Models.Users.User user)
        {
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
        /// <response code="403">The user doesn't have the right permissions.</response>
        [HttpPut]
        [Route("{userId}/avatar")]
        [Authorization(Permissions.MANAGE_USERS)]
        public async Task<ApiResult> ModifyUserAvatar(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return ApiResult.BadRequest();

            if (await _userService.TryModifyUserAvatar(user, Request.Body, Request.ContentLength, Request.ContentType))
            {
                return ApiResult.NoContent();
            }

            return ApiResult.BadRequest();
        }


        /// <summary>
        /// Modify the current user's tags.
        /// Returns the user's current (new) tags.
        /// </summary>
        /// <response code="204">Tags is updated.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">The user isn't authorized.</response>
        [HttpPut]
        [Route("@me/tags")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> ModifySelfTags([FromBody] List<Core.Models.Tags.Tag> tags)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var newTags = _userService.ModifyUserTags(currentUser, tags);
           
            var newTagsModels = _mapper.Map<List<Core.Models.Tags.Tag>>(newTags);

            return ApiResult.Success(newTagsModels);
        }

        /// <summary>
        /// Modify a user's tags.
        /// Returns the user's current (new) tags.
        /// </summary>
        /// <response code="204">Tags are updated.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="403">The user doesn't have the right permissions.</response>
        [HttpPut]
        [Route("{userId}/tags")]
        [Authorization(Permissions.MANAGE_USERS)]
        public async Task<ApiResult> ModifyUserTags(int userId, [FromBody] List<Core.Models.Tags.Tag> tags)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return ApiResult.BadRequest();

            var newTags = _userService.ModifyUserTags(user, tags);
            
            var newTagsModels = _mapper.Map<List<Core.Models.Tags.Tag>>(newTags);

            return ApiResult.Success(newTagsModels);
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

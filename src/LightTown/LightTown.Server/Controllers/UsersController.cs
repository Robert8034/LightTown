using System.Collections.Generic;
using LightTown.Core;
using Microsoft.AspNetCore.Mvc;
using LightTown.Core.Domain.Roles;
using LightTown.Server.Services.Users;
using Microsoft.AspNetCore.StaticFiles;

namespace LightTown.Server.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get a user's avatar.
        /// </summary>
        /// <response code="200">Valid response with avatar image.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">The user isn't authorized.</response>
        /// <response code="404">The avatar doesn't exist.</response>
        [HttpGet]
        [Route("avatars/{avatarFilename}")]
        [Authorization(Permissions.NONE)]
        public IActionResult GetAvatar(string avatarFilename)
        {
            if (_userService.TryGetUserAvatar(avatarFilename, out byte[] avatarBytes))
            {
                if (!new FileExtensionContentTypeProvider().TryGetContentType(avatarFilename, out string contentType))
                    contentType = "application/octet-stream";
                
                return new FileContentResult(avatarBytes, contentType);
            }

            return new NotFoundResult();
        }

    }
}

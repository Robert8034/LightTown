using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Users;
using LightTown.Server.Services.Messages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LightTown.Server.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMessageService _messageService;
        private readonly IMessageLikeService _messageLikeService;

        public MessagesController(UserManager<User> userManager, IMessageService messageService, IMessageLikeService messageLikeService)
        {
            _userManager = userManager;
            _messageService = messageService;
            _messageLikeService = messageLikeService;
        }

        [HttpPut]
        [Route("{messageId}/likes")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> AddMessageLike(int messageId)
        {
            var messageExists = _messageService.MessageExists(messageId);

            if (!messageExists)
                return ApiResult.BadRequest("Message does not exist");

            var user = await _userManager.GetUserAsync(User);

            _messageLikeService.LikeMessage(messageId, user.Id);

            return ApiResult.Success(true);
        }

        [HttpDelete]
        [Route("{messageId}/likes")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> RemoveMessageLike(int messageId)
        {
            var messageExists = _messageService.MessageExists(messageId);

            if (!messageExists)
                return ApiResult.BadRequest("Message does not exist");

            var user = await _userManager.GetUserAsync(User);

            var likeExists = _messageLikeService.LikeExists(messageId, user.Id);

            if (!likeExists)
                return ApiResult.BadRequest("Like does not exist");

            //var messageLike = _messageLikeService.GetMessageLike(messageId, user.Id);

            _messageLikeService.RemoveMessageLike(messageId, user.Id);

            return ApiResult.Success(true);
        }
    }
}

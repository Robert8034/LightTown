using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Domain.Messages;
using LightTown.Core.Domain.Projects;

namespace LightTown.Server.Services.Messages
{
    public interface IMessageLikeService
    {
        void LikeMessage(int messageId, int userId);
        void RemoveMessageLike(MessageLike messageLike);
        int GetMessageLikeCount(int messageId);
        MessageLike GetMessageLike(int messageId, int userId);
        List<MessageLike> GetMessageLikes(int messageId);
        bool LikeExists(int messageId, int userId);
    }
}

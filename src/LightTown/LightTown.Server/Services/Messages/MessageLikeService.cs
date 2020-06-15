using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Data;
using LightTown.Core.Domain.Messages;
using LightTown.Core.Domain.Projects;

namespace LightTown.Server.Services.Messages
{
    public class MessageLikeService : IMessageLikeService
    {
        private readonly IRepository<MessageLike> _messageLikeRepository;
        public MessageLikeService(IRepository<MessageLike> messageLikeRepository)
        {
            _messageLikeRepository = messageLikeRepository;
        }

        public int GetMessageLikeCount(int messageId)
        {
            return _messageLikeRepository.TableNoTracking.Count(e => e.MessageId == messageId);
        }

        public void LikeMessage(int messageId, int userId)
        {
            _messageLikeRepository.Insert(new MessageLike
            {
                MessageId = messageId,
                UserId = userId
            });
        }

        public void RemoveMessageLike(MessageLike messageLike)
        {
            _messageLikeRepository.Delete(messageLike);
        }

        public MessageLike GetMessageLike(int messageId, int userId)
        {
            return _messageLikeRepository.TableNoTracking.SingleOrDefault(e =>
                e.MessageId == messageId && e.UserId == userId);
        }

        public List<MessageLike> GetMessageLikes(int messageId)
        {
            return _messageLikeRepository.TableNoTracking.Where(e => e.MessageId == messageId).ToList();
        }

        public bool LikeExists(int messageId, int userId)
        {
            return _messageLikeRepository.Table.Any(e => e.MessageId == messageId && e.UserId == userId);
        }
    }
}

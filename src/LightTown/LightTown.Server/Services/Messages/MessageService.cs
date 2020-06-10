using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Data;
using LightTown.Core.Domain.Messages;

namespace LightTown.Server.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> _messageRepository;

        public MessageService(IRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public void CreateProjectMessage(int projectId, string title, string content, int userId)
        {
            _messageRepository.Insert(new Message
            {
                ProjectId = projectId,
                Title = title,
                Content = content,
                UserId = userId
            });
        }

        public Message GetMessage(int messageId, int projectId)
        {
            return _messageRepository.TableNoTracking.SingleOrDefault(e =>
                e.ProjectId == projectId && e.Id == messageId);
        }
    }
}

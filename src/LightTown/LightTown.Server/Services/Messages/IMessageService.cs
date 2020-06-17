using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Domain.Messages;

namespace LightTown.Server.Services.Messages
{
    public interface IMessageService
    {
        void CreateProjectMessage(int projectId, string title, string content, string username);
        Message GetMessage(int messageId, int projectId);
        bool MessageExists(int messageId);
    }
}

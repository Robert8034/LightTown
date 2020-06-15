using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LightTown.Client.Services.Messages
{
    public interface IMessageService
    {
        Task<bool> LikeMessage(int messageId);
        Task<bool> RemoveMessageLike(int messageId);
    }
}

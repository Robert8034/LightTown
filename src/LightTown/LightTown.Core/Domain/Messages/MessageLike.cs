using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Data;

namespace LightTown.Core.Domain.Messages
{
    public class MessageLike : BaseEntity
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
    }
}

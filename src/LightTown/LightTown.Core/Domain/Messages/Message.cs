using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Data;

namespace LightTown.Core.Domain.Messages
{
    public class Message : BaseEntity
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public List<MessageLike> MessageLikes { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
    }
}

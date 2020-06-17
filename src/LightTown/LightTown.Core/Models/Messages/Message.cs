using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace LightTown.Core.Models.Messages
{
    [AutoMap(typeof(Domain.Messages.Message))]
    public class Message
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public List<MessageLike> MessageLikes { get; set; }
        public string Content { get; set; }
        public DateTime CreationDateTime { get; set; }
        public int ProjectMessageId { get; set; }
        public string UserName { get; set; }

    }
}

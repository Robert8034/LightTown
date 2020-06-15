using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace LightTown.Core.Models.Messages
{
    [AutoMap(typeof(Domain.Messages.MessageLike))]
    public class MessageLike
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int UserId { get; set; }
    }
}

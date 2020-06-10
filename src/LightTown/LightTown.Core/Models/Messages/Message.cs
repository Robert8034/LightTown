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
        public string Content { get; set; }
    }
}

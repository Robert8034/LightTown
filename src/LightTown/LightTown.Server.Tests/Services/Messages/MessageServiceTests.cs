using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Domain.Messages;
using LightTown.Server.Data;
using LightTown.Server.Services.Messages;
using LightTown.Server.Services.Projects;
using Moq;
using Xunit;

namespace LightTown.Server.Tests.Services.Messages
{
    public class MessageServiceTests
    {
        private readonly Mock<Repository<Message>> _messageRepositoryMock;

        public MessageServiceTests()
        {
            _messageRepositoryMock = new Mock<Repository<Message>>();
        }

        [Fact]
        public void CreateProjectMessageTest()
        {
            //Arrange
            Message expectedMessage = new Message { Id = 1, ProjectId = 1, Title = "New Title", Content = "New Content"};

            _messageRepositoryMock.SetupRepositoryMock(options => { });

            var messageService = new MessageService(_messageRepositoryMock.Object);

            //Act
            messageService.CreateProjectMessage(1, "New Title", "New Content");

            var actualMessage = messageService.GetMessage(1, 1);

            //Assert
            Assert.Equal(expectedMessage.Id, actualMessage.Id);
            Assert.Equal(expectedMessage.ProjectId, actualMessage.ProjectId);

        }
    }
}

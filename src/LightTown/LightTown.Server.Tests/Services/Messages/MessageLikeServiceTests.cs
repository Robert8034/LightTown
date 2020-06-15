using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Domain.Messages;
using LightTown.Core.Domain.Projects;
using LightTown.Server.Data;
using LightTown.Server.Services.Messages;
using LightTown.Server.Services.Projects;
using Moq;
using Xunit;

namespace LightTown.Server.Tests.Services.Messages
{
    public class MessageLikeServiceTests
    {
        private readonly Mock<Repository<MessageLike>> _messageLikeRepositoryMock;

        public MessageLikeServiceTests()
        {
            _messageLikeRepositoryMock = new Mock<Repository<MessageLike>>();
        }

        [Fact]
        public void LikeMessageTest()
        {
            var like1 = new MessageLike { Id = 1, MessageId = 1, UserId = 1 };
            var like2 = new MessageLike { Id = 2, MessageId = 1, UserId = 2 };
            var like3 = new MessageLike { Id = 3, MessageId = 2, UserId = 1 };

            _messageLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var messageLikeService = new MessageLikeService(_messageLikeRepositoryMock.Object);

            var likeCountBefore = messageLikeService.GetMessageLikeCount(1);

            messageLikeService.LikeMessage(1, 3);

            var likeCountAfter = messageLikeService.GetMessageLikeCount(1);

            Assert.Equal(2, likeCountBefore);
            Assert.Equal(3, likeCountAfter);
        }


        [Fact]
        public void LikeExistsTest()
        {
            var like1 = new MessageLike { Id = 1, MessageId = 1, UserId = 1 };
            var like2 = new MessageLike { Id = 2, MessageId = 1, UserId = 2 };
            var like3 = new MessageLike { Id = 3, MessageId = 2, UserId = 1 };

            _messageLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var messageLikeService = new MessageLikeService(_messageLikeRepositoryMock.Object);

            var exists = messageLikeService.LikeExists(1, 1);

            Assert.True(exists);
        }

        [Fact]
        public void LikeExistsFalseTest()
        {
            var like1 = new MessageLike { Id = 1, MessageId = 1, UserId = 1 };
            var like2 = new MessageLike { Id = 2, MessageId = 1, UserId = 2 };
            var like3 = new MessageLike { Id = 3, MessageId = 2, UserId = 1 };

            _messageLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var messageLikeService = new MessageLikeService(_messageLikeRepositoryMock.Object);

            var exists = messageLikeService.LikeExists(7, 8);

            Assert.False(exists);
        }

        [Fact]
        public void LikeExistsFalseMessageTest()
        {
            var like1 = new MessageLike { Id = 1, MessageId = 1, UserId = 1 };
            var like2 = new MessageLike { Id = 2, MessageId = 1, UserId = 2 };
            var like3 = new MessageLike { Id = 3, MessageId = 2, UserId = 1 };

            _messageLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var messageLikeService = new MessageLikeService(_messageLikeRepositoryMock.Object);

            var exists = messageLikeService.LikeExists(7, 1);

            Assert.False(exists);
        }

        [Fact]
        public void LikeExistsFalseUserTest()
        {
            var like1 = new MessageLike { Id = 1, MessageId = 1, UserId = 1 };
            var like2 = new MessageLike { Id = 2, MessageId = 1, UserId = 2 };
            var like3 = new MessageLike { Id = 3, MessageId = 2, UserId = 1 };

            _messageLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var messageLikeService = new MessageLikeService(_messageLikeRepositoryMock.Object);

            var exists = messageLikeService.LikeExists(1, 8);

            Assert.False(exists);
        }

        [Fact]
        public void RemoveMessageLikeTest()
        {
            var like1 = new MessageLike { Id = 1, MessageId = 1, UserId = 1 };
            var like2 = new MessageLike { Id = 2, MessageId = 1, UserId = 2 };
            var like3 = new MessageLike { Id = 3, MessageId = 2, UserId = 1 };

            _messageLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var messageLikeService = new MessageLikeService(_messageLikeRepositoryMock.Object);

            var likeCountBefore = messageLikeService.GetMessageLikeCount(1);

            messageLikeService.RemoveMessageLike(1, 2);

            var likeCountAfter = messageLikeService.GetMessageLikeCount(1);

            Assert.Equal(2, likeCountBefore);
            Assert.Equal(1, likeCountAfter);
        }
    }
}

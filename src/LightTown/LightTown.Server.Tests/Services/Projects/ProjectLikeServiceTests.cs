using System;
using System.Collections.Generic;
using System.Text;
using LightTown.Core.Domain.Projects;
using LightTown.Server.Data;
using LightTown.Server.Services.Projects;
using Moq;
using Xunit;

namespace LightTown.Server.Tests.Services.Projects
{
    public class ProjectLikeServiceTests
    {
        private readonly Mock<Repository<ProjectLike>> _projectLikeRepositoryMock;

        public ProjectLikeServiceTests()
        {
            _projectLikeRepositoryMock = new Mock<Repository<ProjectLike>>();
        }

        [Fact]
        public void LikeProjectTest()
        {
            var like1 = new ProjectLike { Id = 1, ProjectId = 1, UserId = 1 };
            var like2 = new ProjectLike { Id = 2, ProjectId = 1, UserId = 2 };
            var like3 = new ProjectLike { Id = 3, ProjectId = 2, UserId = 1 };

            _projectLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var projectLikeService = new ProjectLikeService(_projectLikeRepositoryMock.Object);

            var likeCountBefore = projectLikeService.GetProjectLikeCount(1);

            projectLikeService.LikeProject(1, 3);

            var likeCountAfter = projectLikeService.GetProjectLikeCount(1);

            Assert.Equal(2, likeCountBefore);
            Assert.Equal(3, likeCountAfter);
        }


        [Fact]
        public void LikeExistsTest()
        {
            var like1 = new ProjectLike { Id = 1, ProjectId = 1, UserId = 1 };
            var like2 = new ProjectLike { Id = 2, ProjectId = 1, UserId = 2 };
            var like3 = new ProjectLike { Id = 3, ProjectId = 2, UserId = 1 };

            _projectLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var projectLikeService = new ProjectLikeService(_projectLikeRepositoryMock.Object);

            var exists = projectLikeService.LikeExists(1, 1);

            Assert.True(exists);
        }

        [Fact]
        public void LikeExistsFalseTest()
        {
            var like1 = new ProjectLike { Id = 1, ProjectId = 1, UserId = 1 };
            var like2 = new ProjectLike { Id = 2, ProjectId = 1, UserId = 2 };
            var like3 = new ProjectLike { Id = 3, ProjectId = 2, UserId = 1 };

            _projectLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var projectLikeService = new ProjectLikeService(_projectLikeRepositoryMock.Object);

            var exists = projectLikeService.LikeExists(7, 8);

            Assert.False(exists);
        }

        [Fact]
        public void LikeExistsFalseProjectTest()
        {
            var like1 = new ProjectLike { Id = 1, ProjectId = 1, UserId = 1 };
            var like2 = new ProjectLike { Id = 2, ProjectId = 1, UserId = 2 };
            var like3 = new ProjectLike { Id = 3, ProjectId = 2, UserId = 1 };

            _projectLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var projectLikeService = new ProjectLikeService(_projectLikeRepositoryMock.Object);

            var exists = projectLikeService.LikeExists(7, 1);

            Assert.False(exists);
        }

        [Fact]
        public void LikeExistsFalseUserTest()
        {
            var like1 = new ProjectLike { Id = 1, ProjectId = 1, UserId = 1 };
            var like2 = new ProjectLike { Id = 2, ProjectId = 1, UserId = 2 };
            var like3 = new ProjectLike { Id = 3, ProjectId = 2, UserId = 1 };

            _projectLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var projectLikeService = new ProjectLikeService(_projectLikeRepositoryMock.Object);

            var exists = projectLikeService.LikeExists(1, 8);

            Assert.False(exists);
        }

        [Fact]
        public void RemoveProjectLikeTest()
        {
            var like1 = new ProjectLike { Id = 1, ProjectId = 1, UserId = 1 };
            var like2 = new ProjectLike { Id = 2, ProjectId = 1, UserId = 2 };
            var like3 = new ProjectLike { Id = 3, ProjectId = 2, UserId = 1 };

            _projectLikeRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(like1);
                options.Insert(like2);
                options.Insert(like3);
            });

            var projectLikeService = new ProjectLikeService(_projectLikeRepositoryMock.Object);

            var likeCountBefore = projectLikeService.GetProjectLikeCount(1);
            
            projectLikeService.RemoveProjectLike(like2);

            var likeCountAfter = projectLikeService.GetProjectLikeCount(1);

            Assert.Equal(2, likeCountBefore);
            Assert.Equal(1, likeCountAfter);
        }
    }
}

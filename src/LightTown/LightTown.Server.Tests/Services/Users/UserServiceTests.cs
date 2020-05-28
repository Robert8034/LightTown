using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Data;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Tags;
using LightTown.Core.Domain.Users;
using LightTown.Server.Data;
using LightTown.Server.Services.Tags;
using LightTown.Server.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LightTown.Server.Tests.Services.Users
{
    public class UserServiceTests
    {
        private readonly Mock<Repository<UserTag>> _userTagRepository;
        private readonly Mock<Repository<Tag>> _tagRepository;
        private Mock<TagService> _tagService;

        public UserServiceTests()
        {
            _userTagRepository = new Mock<Repository<UserTag>>();
            _tagRepository = new Mock<Repository<Tag>>();
        }

        /// <summary>
        /// Test the TryModifyUser method and see if it checks the user data and modifies the user correctly.
        /// </summary>
        [Theory]
        [InlineData(1, "admin", 2, "admin", true)] //user id simply doesn't get changed by the logic so this is still valid
        [InlineData(1, "admin", 0, "admin2", false)] //user can't change their username
        public void TryModifyUserTest(int userId, string username, int newUserId, string newUsername, bool shouldBeValid)
        {
            //Arrange
            var user = new User{Id = userId, UserName = username };

            var userManager = CreateUserManager(options =>
            {
                options.CreateAsync(user).Wait();
            });

            var userModel = new Core.Models.Users.User
            {
                Id = newUserId,
                Username = newUsername
            };

            var userService = new UserService(userManager, null, null, null);

            //Act
            var result = userService.TryModifyUser(userModel, user, out User newUser);

            //Assert
            Assert.Equal(shouldBeValid, result);

            if (shouldBeValid)
            {
                Assert.Equal(newUsername, newUser.UserName);
                Assert.Equal(userId, newUser.Id);
            }
            else
            {
                Assert.Equal(username, user.UserName);
                Assert.Null(newUser);
            }
        }

        /// <summary>
        /// Test the TryModifyUserAvatar method and see if it doesn't allow invalid data.
        /// We only test invalid data since we don't want to create a file in this test.
        /// </summary>
        [Theory]
        [InlineData(10000, "image/test")] //content type cant be invalid
        [InlineData(10000, null)] //content type cant be null
        [InlineData(10000000, "image/png")] //content length needs to less than 8MB
        [InlineData(-10, "image/jpg")] //content length needs to be positive
        [InlineData(0, "image/jpg")] //content length needs to be positive
        public async Task TryModifyUserAvatarTest(long? contentLength, string contentType)
        {
            //Arrange
            var user = new User { Id = 1, HasAvatar = false };

            var userManager = CreateUserManager(options =>
            {
                options.CreateAsync(user).Wait();
            });

            var userService = new UserService(userManager, null, null, null);

            //Act
            var result = await userService.TryModifyUserAvatar(user, null, contentLength, contentType);

            //Assert
            Assert.Equal(false, result);
        }

        /// <summary>
        /// Test the GetUserTagIds method and see if it returns the right tag ids.
        /// </summary>
        [Theory]
        [InlineData(1, new[]{1, 3, 5})]
        [InlineData(2, new[] { 2, 4, 5 })]
        [InlineData(1337, new int[0])]
        public void GetUserTagIdsTest(int userId, int[] expectedTags)
        {
            //Arrange
            var userTag1 = new UserTag
            {
                TagId = 1,
                UserId = 1
            };

            var userTag2 = new UserTag
            {
                TagId = 3,
                UserId = 1
            };

            var userTag3 = new UserTag
            {
                TagId = 5,
                UserId = 1
            };

            var userTag4 = new UserTag
            {
                TagId = 2,
                UserId = 2
            };

            var userTag5 = new UserTag
            {
                TagId = 4,
                UserId = 2
            };

            var userTag6 = new UserTag
            {
                TagId = 5,
                UserId = 2
            };

            _userTagRepository.SetupRepositoryMock(options =>
            {
                options.Insert(userTag1);
                options.Insert(userTag2);
                options.Insert(userTag3);
                options.Insert(userTag4);
                options.Insert(userTag5);
                options.Insert(userTag6);
            });

            var userService = new UserService(null, _userTagRepository.Object, null, null);

            //Act
            var result = userService.GetUserTagIds(userId);

            var firstNotSecond = result.Except(expectedTags).ToList();
            var secondNotFirst = expectedTags.Except(result).ToList();

            //Assert
            Assert.True(!firstNotSecond.Any() && !secondNotFirst.Any());
        }

        /// <summary>
        /// Test the ModifyUserTags method and see if it modifies the right tags.
        /// </summary>
        [Theory]
        [InlineData(1, new[] { 1, 4, 6 }, new[] { 1, 4, 6 })]
        [InlineData(2, new[] { 2, 6, 7 }, new[] { 2, 6, 7 })]
        public void ModifyUserTagsTest(int userId, int[] changedTags, int[] expectedTags)
        {
            //Arrange
            var user = new User
            {
                Id = userId
            };

            var tag1 = new Tag{ Id = 1 };
            var tag2 = new Tag{ Id = 2 };
            var tag3 = new Tag{ Id = 3 };
            var tag4 = new Tag{ Id = 4 };
            var tag5 = new Tag{ Id = 5 };
            var tag6 = new Tag{ Id = 6 };

            var userTag1 = new UserTag
            {
                TagId = 1,
                Tag = tag1,
                UserId = 1
            };

            var userTag2 = new UserTag
            {
                TagId = 3,
                Tag = tag3,
                UserId = 1
            };

            var userTag3 = new UserTag
            {
                TagId = 5,
                Tag = tag5,
                UserId = 1
            };

            var userTag4 = new UserTag
            {
                TagId = 2,
                Tag = tag2,
                UserId = 2
            };

            var userTag5 = new UserTag
            {
                TagId = 4,
                Tag = tag4,
                UserId = 2
            };

            var userTag6 = new UserTag
            {
                TagId = 5,
                Tag = tag5,
                UserId = 2
            };

            var tags = changedTags.Select(tagId => new Core.Models.Tags.Tag
            {
                Id = tagId
            }).ToList();

            var dbContext = new LightTownTestContext();

            _tagRepository.SetupRepositoryMock(dbContext, options =>
            {
                options.Insert(tag1);
                options.Insert(tag2);
                options.Insert(tag3);
                options.Insert(tag4);
                options.Insert(tag5);
                options.Insert(tag6);
            });

            _tagService = new Mock<TagService>(_tagRepository.Object);

            _userTagRepository.SetupRepositoryMock(dbContext, options =>
            {
                options.Insert(userTag1);
                options.Insert(userTag2);
                options.Insert(userTag3);
                options.Insert(userTag4);
                options.Insert(userTag5);
                options.Insert(userTag6);
            });

            var userService = new UserService(null, _userTagRepository.Object, _tagRepository.Object, _tagService.Object);

            //Act
            var newTags = userService.ModifyUserTags(user, tags);

            var firstNotSecond = newTags.Select(tag => tag.Id).Except(expectedTags).ToList();
            var secondNotFirst = expectedTags.Except(newTags.Select(tag => tag.Id)).ToList();

            //Assert
            Assert.True(!firstNotSecond.Any() && !secondNotFirst.Any());

            dbContext.Database.EnsureDeleted();
        }

        private UserManager<User> CreateUserManager(Action<UserManager<User>> options)
        {
            var dbContext = new LightTownTestContext();
            dbContext.Database.EnsureDeleted();

            var userManager = new UserManager<User>(new UserStore<User, Role, LightTownTestContext, int>(dbContext),
                null, null, null, null, null, null, null, null);

            options.Invoke(userManager);

            return userManager;
        }
    }
}

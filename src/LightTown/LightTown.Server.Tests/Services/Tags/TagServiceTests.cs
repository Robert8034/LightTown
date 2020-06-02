using System.Collections.Generic;
using System.Linq;
using LightTown.Core.Domain.Tags;
using LightTown.Server.Data;
using LightTown.Server.Services.Tags;
using Moq;
using Xunit;

namespace LightTown.Server.Tests.Services.Tags
{
    public class TagServiceTests
    {
        private readonly Mock<Repository<Tag>> _tagRepositoryMock;

        public TagServiceTests()
        {
            _tagRepositoryMock = new Mock<Repository<Tag>>();
        }

        /// <summary>
        /// Test the GetTags method and see if it returns all the tags in the database.
        /// </summary>
        [Fact]
        public void GetTagsTest()
        {
            //Arrange
            var tag1 = new Tag {Id = 1, Name = "Tag1"};
            var tag2 = new Tag {Id = 2, Name = "Tag2"};
            var tag3 = new Tag {Id = 3, Name = "Tag3"};

            _tagRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(tag1);
                options.Insert(tag2);
                options.Insert(tag3);
            });

            var tags = new List<Tag>
            {
                tag1,
                tag2,
                tag3
            };

            var tagService = new TagService(_tagRepositoryMock.Object);

            //Act
            var actualTags = tagService.GetTags().ToList();

            //Assert
            for (var i = 0; i < tags.Count; i++)
            {
                Assert.Equal(tags[i].Id, actualTags[i].Id);
                Assert.Equal(tags[i].Name, actualTags[i].Name);
            }
        }

        /// <summary>
        /// Test the GetTag method and see if it return the right tag in the database.
        /// </summary>
        [Theory]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, true)]
        [InlineData(-1, true)]
        public void GetTagTest(int tagId, bool shouldBeNull)
        {
            //Arrange
            var tag1 = new Tag { Id = 1, Name = "Tag1" };
            var tag2 = new Tag { Id = 2, Name = "Tag2" };
            var tag3 = new Tag { Id = 3, Name = "Tag3" };

            _tagRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(tag1);
                options.Insert(tag2);
                options.Insert(tag3);
            });

            var tagService = new TagService(_tagRepositoryMock.Object);

            //Act
            var tag = tagService.GetTag(tagId);

            //Assert
            if (shouldBeNull)
            {
                Assert.Null(tag);
            }
            else
            {
                Assert.Equal(tag.Id, tagId);
            }
        }

        /// <summary>
        /// Test the GetTag method and see if it return the tag in the database.
        /// </summary>
        [Theory]
        [InlineData(0, 4, "name")]
        [InlineData(0, 4, null)]
        [InlineData(1, 4, "name")]
        [InlineData(5, 4, "name")]
        public void InsertTagTest(int tagId, int expectedTagId, string tagName)
        {
            //Arrange
            var tag1 = new Tag { Id = 1, Name = "Tag1" };
            var tag2 = new Tag { Id = 2, Name = "Tag2" };
            var tag3 = new Tag { Id = 3, Name = "Tag3" };

            _tagRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(tag1);
                options.Insert(tag2);
                options.Insert(tag3);
            });

            var tagService = new TagService(_tagRepositoryMock.Object);

            //Act
            var tag = tagService.InsertTag(new Core.Models.Tags.Tag
            {
                Id = tagId,
                Name = tagName
            });

            //Assert
            Assert.Equal(tag.Id, expectedTagId);
            Assert.Equal(tag.Name, tagName);
        }
    }
}

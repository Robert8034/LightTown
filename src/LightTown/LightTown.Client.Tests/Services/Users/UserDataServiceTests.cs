using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LightTown.Client.Services.Users;
using LightTown.Core;
using LightTown.Core.Models.Projects;
using LightTown.Core.Models.Tags;
using LightTown.Core.Models.Users;
using Moq;
using Moq.Protected;
using Xunit;

namespace LightTown.Client.Tests.Services.Users
{
    public class UserDataServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;

        public UserDataServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Default);

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:5001/")
            };
        }

        /// <summary>
        /// Test if LoadData loads all data correctly. This method also tests <see cref="UserDataService.GetCurrentUser"/>.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task LoadDataTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/users/@me", HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new User
            {
                Username = "TestUser",
                TagIds = new List<int>
                {
                    1,
                    3
                }
            }));
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/tags", HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new List<Tag>
            {
                new Tag{Id = 1},
                new Tag{Id = 2},
                new Tag{Id = 3},
                new Tag{Id = 4},
            }));

            var userDataService = new UserDataService(_httpClient, null);

            //ACT
            await userDataService.LoadData();

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/users/@me")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/tags")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Equal("TestUser", userDataService.GetCurrentUser().Username);
            Assert.Equal(2, userDataService.GetCurrentUser().Tags.Count);
            Assert.Equal(1, userDataService.GetCurrentUser().Tags[0].Id);
            Assert.Equal(3, userDataService.GetCurrentUser().Tags[1].Id);
        }

        /// <summary>
        /// Tests whether <see cref="UserDataService.UnloadData"/> unloads all data.
        /// </summary>
        [Fact]
        public async Task UnloadDataTest()
        {
            //ARRANGE
            var userSessionService = new UserDataService(_httpClient, null);

            //ACT
            await userSessionService.UnloadData();

            //ASSERT
            Assert.Null(userSessionService.GetCurrentUser());
        }

        /// <summary>
        /// Tests whether <see cref="UserDataService.GetProjects"/> gets all projects and if it caches the projects.
        /// </summary>
        [Fact]
        public async Task GetProjectsTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/projects",HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new List<Project>()
                {
                    new Project
                    {
                        ProjectName = "TestProject"
                    }
                }
            ));

            var userDataService = new UserDataService(_httpClient, null);

            //ACT
            //call GetProjects() twice so we not only get the projects from the "server" but also from the cache.
            var projects = await userDataService.GetProjects();
            var projectsAgain = await userDataService.GetProjects();

            //ASSERT
            //even though GetProjects() was called twice, we only expect it to actually retrieve data from the server the first time (so only once).
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/projects")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Equal("TestProject", projects[0].ProjectName);

            //we expect both projects to be the same object since the second GetProjects() call simply returned the cached projects list.
            Assert.Equal(projectsAgain[0], projects[0]);
        }

        /// <summary>
        /// Tests whether <see cref="UserDataService.GetProject"/> gets the project and if it caches the project.
        /// </summary>
        [Theory]
        [InlineData(1, false)]
        [InlineData(1337, true)]
        public async Task GetProjectTest(int projectId, bool shouldBeNull)
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/projects/1",HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Project
                {
                    Id = 1,
                    ProjectName = "TestProject",
                    ProjectDescription = "TestDescription",
                    TagIds = new List<int>
                    {
                        2
                    }
                }
            ));

            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/projects/1337", HttpStatusCode.NotFound, new ApiResult(HttpStatusCode.NotFound, null));

            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/tags/2", HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Tag { Id = 2 }));

            var userDataService = new UserDataService(_httpClient, null);

            //ACT
            //call GetProject() twice so we not only get the project from the "server" but also from the cache.
            var project = await userDataService.GetProject(projectId);
            var projectAgain = await userDataService.GetProject(projectId);

            //ASSERT
            //even though GetProject() was called twice, we only expect it to actually retrieve data from the server the first time (so only once), assuming it cached the project.
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(shouldBeNull ? 2 : 1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri($"https://localhost:5001/api/projects/{projectId}")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            if (shouldBeNull)
            {
                Assert.Null(project);
                Assert.Null(projectAgain);
            }
            else
            {
                Assert.Equal("TestProject", project.ProjectName);
                Assert.Equal(1, project.Tags.Count);
                Assert.Equal(2, project.Tags[0].Id);

                //we expect both projects to be the same object since the second GetProject() call simply returned the cached project.
                Assert.Equal(projectAgain, project);
            }

        }

        /// <summary>
        /// Tests whether <see cref="UserDataService.GetUser"/> gets the user and if it caches the user.
        /// </summary>
        [Theory]
        [InlineData(1, false)]
        [InlineData(1337, true)]
        public async Task GetUserTest(int userId, bool shouldBeNull)
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/users/1", HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new User
            {
                Id = 1,
                Username = "admin",
                TagIds = new List<int>
                {
                    2, 3
                }
            }
            ));

            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/users/1337", HttpStatusCode.NotFound, null);

            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/tags/2", HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Tag { Id = 2 }));
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/tags/3", HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Tag { Id = 3 }));

            var userDataService = new UserDataService(_httpClient, null);

            //ACT
            //call GetUser() twice so we not only get the user from the "server" but also from the cache.
            var user = await userDataService.GetUser(userId);
            var userAgain = await userDataService.GetUser(userId);

            //ASSERT
            //even though GetUser() was called twice, we only expect it to actually retrieve data from the server the first time (so only once), assuming it cached the user.
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(shouldBeNull ? 2 : 1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri($"https://localhost:5001/api/users/{userId}")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            if (shouldBeNull)
            {
                Assert.Null(user);
                Assert.Null(userAgain);
            }
            else
            {
                Assert.Equal("admin", user.Username);
                Assert.Equal(2, user.Tags.Count);
                Assert.Equal(2, user.Tags[0].Id);

                //we expect both users to be the same object since the second GetUser() call simply returned the cached user.
                Assert.Equal(userAgain, user);
            }
        }

        /// <summary>
        /// Tests whether <see cref="UserDataService.GetProjectMembers"/> gets the project members and if it caches the users.
        /// </summary>
        [Theory]
        [InlineData(1, false)]
        [InlineData(1337, true)]
        public async Task GetProjectMembersTest(int projectId, bool shouldBeNull)
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/projects/1", HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Project
            {
                Id = 1
            }
            ));

            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/projects/1337", HttpStatusCode.NotFound, new ApiResult(HttpStatusCode.NotFound, null));

            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/projects/1/members", HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new List<User>
                {
                    new User
                    {
                        Id = 1
                    },
                    new User
                    {
                        Id = 3
                    },
                }
            ));

            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/users/3", HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new User
                {
                    Id = 3
                }
            ));

            var userDataService = new UserDataService(_httpClient, null);

            //ACT
            //call GetProjectMembers() twice so we not only get the project members from the "server" but also from the cache.
            var projectMembers = await userDataService.GetProjectMembers(projectId);
            var projectMembersAgain = await userDataService.GetProjectMembers(projectId);

            var user = await userDataService.GetUser(3);

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(shouldBeNull ? 2 : 1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri($"https://localhost:5001/api/projects/{projectId}")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            //even though GetProjectMembers() was called twice, we only expect it to actually retrieve data from the server the first time (so only once).
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(shouldBeNull ? 0 : 1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri($"https://localhost:5001/api/projects/{projectId}/members")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            if (shouldBeNull)
            {
                Assert.Null(projectMembers);
                Assert.Null(projectMembersAgain);
            }
            else
            {
                Assert.Equal(2, projectMembers.Count);
                Assert.Equal(1, projectMembers[0].Id);
                Assert.Equal(3, projectMembers[1].Id);

                //we expect both project members to be the same object since the second GetProjectMembers() call simply returned the cached project members.
                Assert.Equal(projectMembers[0], projectMembersAgain[0]);
            }

            Assert.Equal(3, user.Id);
        }

        /// <summary>
        /// Tests whether <see cref="UserDataService.GetTag"/> gets the tag and if it caches the tag.
        /// </summary>
        [Theory]
        [InlineData(3, false)]
        [InlineData(1337, true)]
        public async Task GetTagTest(int tagId, bool shouldBeNull)
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/tags/3", HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Tag
            {
                Id = 3
            }));

            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/tags/1337", HttpStatusCode.NotFound, new ApiResult(HttpStatusCode.NotFound, null));

            var userDataService = new UserDataService(_httpClient, null);

            //ACT
            //call GetTag() twice so we not only get the project members from the "server" but also from the cache.
            var tag = await userDataService.GetTag(tagId);
            var tagAgain = await userDataService.GetTag(tagId);

            //ASSERT
            //even though GetTag() was called twice, we only expect it to actually retrieve data from the server the first time (so only once) IF the tag exists (not 404), otherwise it will try to get it from the server again.
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(shouldBeNull ? 2 : 1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri($"https://localhost:5001/api/tags/{tagId}")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            if (shouldBeNull)
            {
                Assert.Null(tag);
                Assert.Null(tagAgain);
            }
            else
            {
                Assert.Equal(3, tag.Id);

                //we expect both tags to be the same object since the second GetTag() call simply returned the cached tag.
                Assert.Equal(tag, tagAgain);
            }
        }
    }
}
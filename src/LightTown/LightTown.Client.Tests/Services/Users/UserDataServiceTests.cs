using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LightTown.Client.Services.Users;
using LightTown.Core;
using LightTown.Core.Models.Projects;
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
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

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
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new User
            {
                Username = "TestUser"
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

            Assert.Equal("TestUser", userDataService.GetCurrentUser().Username);
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

        [Fact]
        public async Task GetProjectsTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new List<Project>()
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

        [Fact]
        public async Task GetProjectTest()
        {
            int projectId = 1;

            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Project
                {
                    Id = 1,
                    ProjectName = "TestProject",
                    ProjectDescription = "TestDescription"
                }
            ));

            var userDataService = new UserDataService(_httpClient, null);

            //ACT
            //call GetProject() twice so we not only get the projects from the "server" but also from the cache.
            var project = await userDataService.GetProject(projectId);
            var projectAgain = await userDataService.GetProject(projectId);

            //ASSERT
            //even though GetProject() was called twice, we only expect it to actually retrieve data from the server the first time (so only once).
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri($"https://localhost:5001/api/projects/{projectId}")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Equal("TestProject", project.ProjectName);

            //we expect both projects to be the same object since the second GetProjects() call simply returned the cached projects list.
            Assert.Equal(projectAgain, project);
        }
    }
}
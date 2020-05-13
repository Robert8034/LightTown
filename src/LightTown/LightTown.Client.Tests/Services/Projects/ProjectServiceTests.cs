using LightTown.Client.Services.Projects;
using LightTown.Client.Services.Users;
using LightTown.Core;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Models.Projects;
using LightTown.Core.Models.Users;
using Microsoft.JSInterop;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LightTown.Client.Tests.Services.Projects
{
    public class ProjectServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private Mock<IJSRuntime> _ijsRuntimeMock;
        private Mock<MockNavigationManager> _navigationManagerMock;

        public ProjectServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:5001/")
            };

            _ijsRuntimeMock = new Mock<IJSRuntime>(MockBehavior.Strict);

            _navigationManagerMock = new Mock<MockNavigationManager>(MockBehavior.Loose);
        }

        [Fact]
        public async Task GetProjectsTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new User
            {
                Username = "TestUser"
            }));

            var userSessionService = new UserSessionService(_httpClient, null, null);

            await userSessionService.LoadUser();

            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new List<Project>()
            {
                new Project()
                {
                    ProjectName = "TestProject"
                }
            }
            ));

            var projectService = new ProjectService(_httpClient);

            //ACT
            List<Project> projects = await projectService.GetProjects();

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/projects")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Equal("TestProject", projects[0].ProjectName);

        }

        [Fact]
        public async Task CreateProjectTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Project()
            {
               ProjectName = "TestProject",
               ProjectDescription = "TestDescription"
            }));

            var projectService = new ProjectService(_httpClient);

            //ACT
            Project project = await projectService.CreateProject("TestProject", "TestDescription");

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
             ItExpr.Is<HttpRequestMessage>(req =>
                 req.Method == HttpMethod.Post
                 && req.RequestUri == new Uri("https://localhost:5001/api/projects") 
             ),
             ItExpr.IsAny<CancellationToken>()
             );

            Assert.Equal("TestProject", project.ProjectName);
            Assert.Equal("TestDescription", project.ProjectDescription);

        }

        [Fact]
        public async Task GetProjectTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Project()
            {
                Id = 1,
                ProjectName = "TestProject",
                ProjectDescription = "TestDescription",
                Members = new List<User>()
                {
                    new User()
                    {
                        Username = "test"
                    }
                }
            }));

            var projectService = new ProjectService(_httpClient);

            //ACT
            Project project = await projectService.GetProject(1);

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
             ItExpr.Is<HttpRequestMessage>(req =>
                 req.Method == HttpMethod.Get
                 && req.RequestUri == new Uri("https://localhost:5001/api/projects/1")
             ),
             ItExpr.IsAny<CancellationToken>()
             );

            Assert.Equal(1, project.Id);
        }

        [Fact]
        public async Task RemoveMemberTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, true));

            var projectService = new ProjectService(_httpClient);

            //ACT
            var result = await projectService.RemoveMember(1, 1);

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
             ItExpr.Is<HttpRequestMessage>(req =>
                 req.Method == HttpMethod.Get
                 && req.RequestUri == new Uri("https://localhost:5001/api/projects/1/1/remove")
             ),
             ItExpr.IsAny<CancellationToken>()
             );

            Assert.True(result);
        }

        [Fact]
        public async Task AddMemberTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, true));

            var projectService = new ProjectService(_httpClient);

            //ACT
            var result = await projectService.AddMember(1, 1);

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
             ItExpr.Is<HttpRequestMessage>(req =>
                 req.Method == HttpMethod.Put
                 && req.RequestUri == new Uri("https://localhost:5001/api/projects/1/1")
             ),
             ItExpr.IsAny<CancellationToken>()
             );

            Assert.True(result);
        }
    }
}

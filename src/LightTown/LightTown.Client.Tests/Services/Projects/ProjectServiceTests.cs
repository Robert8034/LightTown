using LightTown.Client.Services.Projects;
using LightTown.Client.Services.Users;
using LightTown.Core;
using LightTown.Core.Models.Projects;
using LightTown.Core.Models.Users;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LightTown.Client.Tests.Services.Projects
{
    public class ProjectServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;

        public ProjectServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:5001/")
            };
        }
        
        [Fact]
        public async Task CreateProjectTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Post, "api/projects",HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Project()
            {
               ProjectName = "TestProject",
               ProjectDescription = "TestDescription"
            }));

            var projectService = new ProjectService(_httpClient, null);

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

        //[Fact]
        //public async Task GetProjectTest()
        //{
        //    //ARRANGE
        //    _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(HttpStatusCode.OK, new Project()
        //    {
        //        Id = 1,
        //        ProjectName = "TestProject",
        //        ProjectDescription = "TestDescription",
        //        Members = new List<User>()
        //        {
        //            new User()
        //            {
        //                Username = "test"
        //            }
        //        }
        //    }));

        //    var projectService = new ProjectService(_httpClient, null);

        //    //ACT
        //    Project project = await projectService.GetProject(1);

        //    //ASSERT
        //    _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
        //     ItExpr.Is<HttpRequestMessage>(req =>
        //         req.Method == HttpMethod.Get
        //         && req.RequestUri == new Uri("https://localhost:5001/api/projects/1")
        //     ),
        //     ItExpr.IsAny<CancellationToken>()
        //     );

        //    Assert.Equal(1, project.Id);
        //}

        [Fact]
        public async Task RemoveMemberTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Delete, "api/projects/1/members/1",HttpStatusCode.NoContent, ApiResult.NoContent());

            var projectService = new ProjectService(_httpClient, null);

            //ACT
            var result = await projectService.RemoveMember(1, 1);

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
             ItExpr.Is<HttpRequestMessage>(req =>
                 req.Method == HttpMethod.Delete
                 && req.RequestUri == new Uri("https://localhost:5001/api/projects/1/members/1")
             ),
             ItExpr.IsAny<CancellationToken>()
             );

            Assert.True(result);
        }

        [Fact]
        public async Task AddMemberTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Put, "api/projects/1/members/1", HttpStatusCode.OK, ApiResult.NoContent());

            var projectService = new ProjectService(_httpClient, null);

            //ACT
            var result = await projectService.AddMember(1, 1);

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put
                    && req.RequestUri == new Uri("https://localhost:5001/api/projects/1/members/1")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.True(result);
        }

        [Fact]
        public async Task GetProjectMembersTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpMethod.Get, "api/projects/1/members",HttpStatusCode.NoContent, new ApiResult(
                HttpStatusCode.OK, new List<User>
                {
                    new User
                    {
                        Id = 1,
                        Username = "Test"
                    },
                    new User
                    {
                        Id = 2,
                        Username = "Test2"
                    }
                }));

            var projectService = new ProjectService(_httpClient, null);

            //ACT
            var result = await projectService.GetProjectMembers(1);

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/projects/1/members")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Equal(1, result[0].Id);
            Assert.Equal(2, result[1].Id);
            Assert.Equal("Test", result[0].Username);
            Assert.Equal("Test2", result[1].Username);
        }

        [Fact]
        public async Task SearchProjectsTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(
                HttpStatusCode.OK, new List<Project>
                {
                   new Project
                   {
                       ProjectName = "Test",
                       ProjectDescription = "TestDesc",
                       Id = 1
                   }
                }));

            var projectService = new ProjectService(_httpClient, null);

            //ACT
            var result = await projectService.SearchProjects("Test");

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/projects/search/Test")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Equal(1, result[0].Id);
            Assert.Equal("Test", result[0].ProjectName);
            Assert.Equal("TestDesc", result[0].ProjectDescription);
        }
    }
}

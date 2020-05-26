using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LightTown.Client.Services.Projects;
using LightTown.Client.Services.Users;
using LightTown.Core;
using LightTown.Core.Models.Users;
using Moq;
using Moq.Protected;
using Xunit;

namespace LightTown.Client.Tests.Services.Users
{
    public class UserServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;

        public UserServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:5001/")
            };
        }

        [Fact]
        public async Task SearchUsersTest()
        {
            //ARRANGE
            _httpMessageHandlerMock.SetupHttpMessageHandlerMock(HttpStatusCode.OK, new ApiResult(
                HttpStatusCode.OK, new List<User>
                {
                    new User
                    {
                        Id = 1,
                        Username = "Test"
                    }
                }));

            var userService = new UserService(_httpClient, null);

            //ACT
            var result = await userService.SearchUsers("Test");

            //ASSERT
            _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:5001/api/users/search/Test")
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Equal(1, result[0].Id);
            Assert.Equal("Test", result[0].Username);
        }
    }
}

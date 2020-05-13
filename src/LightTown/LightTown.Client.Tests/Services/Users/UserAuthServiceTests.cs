using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using LightTown.Client.Services.Users;
using LightTown.Core;
using LightTown.Core.Models.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

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

            var userDataService = new UserDataService(_httpClient);

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
        public void UnloadDataTest()
        {
            //ARRANGE
            var userSessionService = new UserDataService(_httpClient);

            //ACT
            userSessionService.UnloadData();

            //ASSERT
            Assert.Null(userSessionService.GetCurrentUser());
        }
    }

    public class UserAuthServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private Mock<IJSRuntime> _ijsRuntimeMock;
        private Mock<MockNavigationManager> _navigationManagerMock;

        public UserAuthServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:5001/")
            };

            _ijsRuntimeMock = new Mock<IJSRuntime>(MockBehavior.Strict);

            _navigationManagerMock = new Mock<MockNavigationManager>( MockBehavior.Loose);
        }

        /// <summary>
        /// Test whether TryLoadAuthentication returns the right value based on the cookie value it should get from the client.
        /// </summary>
        /// <param name="cookie">The cookie to test with.</param>
        /// <param name="isValid">If the cookie is expected to be valid.</param>
        /// <returns></returns>
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("test", false)]
        [InlineData(".AspNetCore.Identity.Application=CfDJ8Dxi8SoNHMxCgS_gub9OejXxSkrDzPalNO-B0f23jNqIMP_FNBBj2bptK4MnST2WQ6dAG6GaWeP12uCPp6SPez2JgKx5S0cPLARg7eXyTousoxuk65IijlEy_-mXbtZOCmXxsF1sHIUjxQiZG2-HcE45485agdKuY2nFJltQwXQ-49YL4Uqg2NuAutdZf_UVlKMfRWUe-Dn_78_JmzqynjTa7dyDnIZAp7l8D5LIvCmxlb2M2ukXKTq6Jphgs5nnbgHJhJRIUiH9CFaC8enDD9lVCKu65kWOxCIttbBEYxuD7lae7n7z1gEumAO3Cg0yxnDV7YB9OiZFXUeJ-fWNbnQb1PkxQQ4Zle4PuzdI0UFJD8JjMQYaPLJnwmNAzbO2ks2bIsRXpD9Ge-yzS0odNZPjPgQHtMcApMfzlOVCgOGyG068kP3cKX1RTfjvb6S-6Te_sbVtsOBihjA9sFmMNb1BN9l9pD1CsbVEH8P_VJorlZqcxIPJpNNycf4GPP75uGkje5rH21F0YDeSZS_HHHDCF_fvMUWVbaedvGnMZdp6yhYFWhfh1QePAvpdr-uZ2LO6hPiDG5Wbo0gNl-NmpJ0qxKUYAWyxXtbOdhm00gWtaSpcBJF-jDpeO52ZAUUG_VAYNH_kps7Pam4B7MrkiShJJRHn7TXFCs_xb69PW7jihHtBccYU-w2V1CWTAiM4tg", true)]
        public async Task TryLoadAuthenticationTest(string cookie, bool isValid)
        {
            //ARRANGE
            _ijsRuntimeMock.SetupIjsRuntimeMock<string>("getCookies", cookie);

            var userAuthService = new UserAuthService(_ijsRuntimeMock.Object, _navigationManagerMock.Object, null);

            //ACT
            bool result = await userAuthService.TryLoadAuthentication();

            //ASSERT
            Assert.Equal(isValid, result);
        }
    }
}

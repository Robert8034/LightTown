using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LightTown.Client.Services.Users;
using LightTown.Core;
using LightTown.Core.Models.Users;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace LightTown.Client.Tests.Services.Users
{
    public class UserSessionSericeTests
    {
        public class HttpClientProxy
        {
            public T GetJsonAsync<T>()
            {
                return default;
            }
        }

        [Fact]
        public async Task Test1Async()
        {
            //var moq = new Mock<UserSessionService>();
            var moqHttpClient = new Mock<HttpClient>();

            //moqHttpClient.Protected().As<IHttpMessageHandler>().Setup(httpClient => httpClient.SendAsync(It.IsAny<HttpRequestMessage>()))
            //    .ReturnsAsync(() => new HttpResponseMessage()
            //    {
            //        StatusCode = HttpStatusCode.OK,
            //        Content = new StringContent(JsonConvert.SerializeObject(new ApiResult(HttpStatusCode.OK, new User
            //        {
            //            Username = "TestUser"
            //        })))
            //    });
            //moq.Setup(userSessionService => userSessionService.LoadUser()).Returns(moqHttpWebRequest.Object);

            var retriever = new UserSessionService(moqHttpClient.Object, null, null);

            await retriever.LoadUser();

            moqHttpClient.Verify();
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace LightTown.Client.Tests
{
    public static class MockExtensions
    {
        /// <summary>
        /// Setup the HttpMessageHandler Mock with expected return data.
        /// </summary>
        /// <param name="httpMessageHandlerMock"></param>
        /// <param name="method">The http method to mock.</param>
        /// <param name="url">The url to call.</param>
        /// <param name="httpStatusCode">The expected status code.</param>
        /// <param name="content">The expected content which will be serialized to JSON.</param>
        public static void SetupHttpMessageHandlerMock(this Mock<HttpMessageHandler> httpMessageHandlerMock, HttpMethod method, string url, HttpStatusCode httpStatusCode, object content)
        {

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(httpRequestMessage =>
                        httpRequestMessage.Method == method && httpRequestMessage.RequestUri == new Uri("https://localhost:5001/" + url)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode,
                    Content = new StringContent(JsonConvert.SerializeObject(content))
                })
                .Verifiable();
        }

        /// <summary>
        /// Setup the IJSRuntime Mock with method name and expected return data.
        /// </summary>
        /// <param name="ijsRuntimeMock"></param>
        /// <param name="identifier">The method name to call.</param>
        /// <param name="returnData">The data to return.</param>
        /// <param name="args">The optional arguments for the method.</param>
        public static void SetupIjsRuntimeMock<T>(this Mock<IJSRuntime> ijsRuntimeMock, string identifier, T returnData, params object[] args)
        {
            ijsRuntimeMock
                .Setup(runtime => runtime.InvokeAsync<T>(identifier, args))
                .ReturnsAsync(returnData)
                .Verifiable();
        }
    }
}

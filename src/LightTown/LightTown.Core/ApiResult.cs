using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Json.Internal;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LightTown.Core
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiResult : ActionResult, IStatusCodeActionResult
    {
        public int? StatusCode { get; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public JToken Data { get; set; }

        public ApiResult(HttpStatusCode statusCode, object value)
        {
            StatusCode = (int)statusCode;
            Data = value == null ? null : JToken.FromObject(value);
        }

        /// <summary>
        /// Get the JSON data from the ApiResult response.
        /// <para>
        /// Returns <see langword="default"/> of <typeparamref name="T"/> if <see cref="ApiResult.Data"/> is <see langword="null"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetData<T>()
        {
            return Data != null ? Data.ToObject<T>() : default;
        }

        /// <summary>
        /// Creates an ApiResult with status code 200 (Success).
        /// </summary>
        /// <param name="data">The data to show in the JSON response.</param>
        /// <param name="message">The message to show in the JSON response.</param>
        /// <returns></returns>
        public static ApiResult Success(object data, string message)
        {
            var responseObject = new
            {
                isSuccess = true,
                message,
                data
            };

            return new ApiResult(HttpStatusCode.OK, responseObject);
        }

        /// <summary>
        /// Creates an ApiResult with status code 200 (Success).
        /// </summary>
        /// <param name="data">The data to show in the JSON response.</param>
        /// <returns></returns>
        public static ApiResult Success(object data)
        {
            var responseObject = new
            {
                isSuccess = true,
                data
            };

            return new ApiResult(HttpStatusCode.OK, responseObject);
        }

        /// <summary>
        /// Creates an ApiResult with status code 204 (No Content).
        /// </summary>
        /// <returns></returns>
        public static ApiResult NoContent() => new ApiResult(HttpStatusCode.NoContent, null);

        /// <summary>
        /// Creates an ApiResult with status code 404 (Not Found).
        /// </summary>
        /// <returns></returns>
        public static ApiResult NotFound()
        {
            var responseObject = new
            {
                isSuccess = false
            };

            return new ApiResult(HttpStatusCode.NotFound, responseObject);
        }

        /// <summary>
        /// Creates an ApiResult with status code 404 (Not Found).
        /// </summary>
        /// <param name="message">The message to show in the JSON response.</param>
        /// <returns></returns>
        public static ApiResult NotFound(string message)
        {
            var responseObject = new
            {
                isSuccess = false,
                message
            };

            return new ApiResult(HttpStatusCode.NotFound, responseObject);
        }

        /// <summary>
        /// Creates an ApiResult with status code 400 (Bad Request).
        /// </summary>
        /// <returns></returns>
        public static ApiResult BadRequest()
        {
            var responseObject = new
            {
                isSuccess = false
            };

            return new ApiResult(HttpStatusCode.BadRequest, responseObject);
        }

        /// <summary>
        /// Creates an ApiResult with status code 400 (Bad Request).
        /// </summary>
        /// <param name="message">The message to show in the JSON response.</param>
        /// <returns></returns>
        public static ApiResult BadRequest(string message)
        {
            var responseObject = new
            {
                isSuccess = false,
                message
            };

            return new ApiResult(HttpStatusCode.BadRequest, responseObject);
        }

        /// <summary>
        /// Creates an ApiResult with status code 403 (Forbidden).
        /// </summary>
        /// <param name="message">The message to show in the JSON response.</param>
        /// <returns></returns>
        public static ApiResult Forbidden(string message)
        {
            var responseObject = new
            {
                isSuccess = false,
                message
            };

            return new ApiResult(HttpStatusCode.Forbidden, responseObject);
        }

        /// <summary>
        /// Creates an ApiResult with status code 304 (Not Modified).
        /// </summary>
        /// <param name="message">The message to show in the JSON response.</param>
        /// <returns></returns>
        public static ApiResult NotModified(string message)
        {
            var responseObject = new
            {
                isSuccess = true,
                message
            };

            return new ApiResult(HttpStatusCode.NotModified, responseObject);
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            response.StatusCode = StatusCode.Value;

            //only send JSON body when StatusCode isn't 204 (No Content)
            if (StatusCode != 204)
            {
                response.ContentType = "application/json; charset=utf-8";

                var body = Encoding.UTF8.GetBytes(Data.ToString());
                await response.Body.WriteAsync(body, 0, body.Length);

                response.ContentLength = body.Length;
            }
        }
    }
}

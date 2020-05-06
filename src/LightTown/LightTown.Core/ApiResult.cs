using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LightTown.Core
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiResult : JsonResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public JToken Data { get; set; }

        public ApiResult(HttpStatusCode statusCode, object value) : base(value)
        {
            StatusCode = (int)statusCode;
            Data = value == null ? null : JToken.FromObject(value);
        }

        public T GetData<T>()
        {
            return Data != null ? Data.ToObject<T>() : default;
        }

        public static ApiResult Success(object data, string message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var responseObject = new
                {
                    isSuccess = true,
                    message,
                    data
                };

                return new ApiResult(HttpStatusCode.OK, responseObject);
            }
            else
            {
                var responseObject = new
                {
                    isSuccess = true,
                    data
                };

                return new ApiResult(HttpStatusCode.OK, responseObject);
            }
        }

        public static ApiResult NoContent() => new ApiResult(HttpStatusCode.NoContent, null);

        public static ApiResult NotFound()
        {
            var responseObject = new
            {
                isSuccess = false
            };

            return new ApiResult(HttpStatusCode.NotFound, responseObject);
        }

        public static ApiResult NotFound(string message)
        {
            var responseObject = new
            {
                isSuccess = false,
                message
            };

            return new ApiResult(HttpStatusCode.NotFound, responseObject);
        }

        public static ApiResult BadRequest()
        {
            var responseObject = new
            {
                isSuccess = false
            };

            return new ApiResult(HttpStatusCode.BadRequest, responseObject);
        }

        public static ApiResult BadRequest(string message)
        {
            var responseObject = new
            {
                isSuccess = false,
                message
            };

            return new ApiResult(HttpStatusCode.BadRequest, responseObject);
        }

        public static ApiResult Forbidden(string message)
        {
            var responseObject = new
            {
                isSuccess = false,
                message
            };

            return new ApiResult(HttpStatusCode.Forbidden, responseObject);
        }

        public static ApiResult NotModified(string message)
        {
            var responseObject = new
            {
                isSuccess = true,
                message
            };

            return new ApiResult(HttpStatusCode.NotModified, responseObject);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace LightTown.Server
{
    public class ApiResult : JsonResult
    {
        //public int StatusCode { get; set; }
        public object ResponseObject { get; set; }

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter()
            }
        };

        //public async Task ExecuteResultAsync(ActionContext context)
        //{
        //    context.HttpContext.Response.StatusCode = StatusCode;

        //    context.HttpContext.Response.ContentType = "application/json";

        //    if (ResponseObject != null)
        //        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(ResponseObject, _serializerSettings));
        //}

        public ApiResult(HttpStatusCode statusCode, object value) : base(value)
        {
            StatusCode = (int)statusCode;
            ResponseObject = value;
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
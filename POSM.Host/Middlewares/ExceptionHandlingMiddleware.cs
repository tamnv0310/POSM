using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using POSM.Host.Helpers;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace POSM.Host.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private string requestBodyText = string.Empty;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                #region get request

                var requestBodyStream = new MemoryStream();
                await context.Request.Body.CopyToAsync(requestBodyStream);
                requestBodyStream.Seek(0, SeekOrigin.Begin);

                requestBodyText = await new StreamReader(requestBodyStream).ReadToEndAsync();
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = requestBodyStream;

                #endregion

                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, requestBodyText, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string requestBody, Exception exception)
        {
            if (exception == null)
                return;

            var code = HttpStatusCode.InternalServerError;
            var message = "";

            switch (exception)
            {
                case UnauthorizedAccessException _:
                    code = HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    break;
                default:
                    break;
            }

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;
            var obj = ApiHelper.ToObjectResult(false, null, message, code).Value;

            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await response
                .WriteAsync(json)
                .ConfigureAwait(false);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace POSM.Host.Helpers
{
    public static class ApiHelper
    {
        public static ObjectResult ToObjectResult(bool success = true, object data = null,
            string message = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            var apiResponse = new ApiResponse()
            {
                Status = success ? 1 : 0,
                Data = data,
                Message = message,

            };
            return new ObjectResult(apiResponse) { StatusCode = (int)httpStatusCode };
        }

        public static ObjectResult Created(string successMsg = null, string url = null, Object data = null)
        {
            return ToObjectResult(true, data, successMsg, HttpStatusCode.Created);
        }

        public static ObjectResult Success(string successMsg = null, Object data = null)
        {
            return ToObjectResult(true, data, successMsg, HttpStatusCode.OK);
        }

        public static ObjectResult Created(string successMsg, object data = null)
        {
            return ToObjectResult(true, data, successMsg, HttpStatusCode.Created);
        }

        public static ObjectResult BadRequest(ModelStateDictionary modelState)
        {
            string errorMsg = null;
            var error = modelState.SelectMany(x => x.Value.Errors).First();
            if (!string.IsNullOrEmpty(error.ErrorMessage))
                errorMsg = error.ErrorMessage;
            else if (error.Exception?.Message != null)
                errorMsg = error.Exception.Message;

            return ToObjectResult(false, null, errorMsg, HttpStatusCode.BadRequest);
        }

        public static ObjectResult BadRequest(string errorMsg)
        {
            return ToObjectResult(false, null, errorMsg, HttpStatusCode.BadRequest);
        }

        public static ObjectResult BadRequest(ModelStateDictionary modelState, object data = null)
        {
            string errorMsg = null;
            var error = modelState.SelectMany(x => x.Value.Errors).First();
            if (!string.IsNullOrEmpty(error.ErrorMessage))
                errorMsg = error.ErrorMessage;
            else if (error.Exception?.Message != null)
                errorMsg = error.Exception.Message;

            return ToObjectResult(false, data, errorMsg, HttpStatusCode.BadRequest);
        }

        public static ObjectResult BadRequest(string errorMsg, object data = null)
        {
            return ToObjectResult(false, data, errorMsg, HttpStatusCode.BadRequest);
        }

        public static ObjectResult Conflict(string errorMsg)
        {
            return ToObjectResult(false, null, errorMsg, HttpStatusCode.Conflict);
        }

        public static ObjectResult NotFound(string errorMsg)
        {
            return ToObjectResult(false, null, errorMsg, HttpStatusCode.NotFound);
        }

        public static ObjectResult ExpectationFailed(string errorMsg)
        {
            return ToObjectResult(false, null, errorMsg, HttpStatusCode.ExpectationFailed);
        }

        public static ObjectResult Unauthorized(string errorMsg = "Unauthorized")
        {
            return ToObjectResult(false, null, errorMsg, HttpStatusCode.Unauthorized);
        }
    }

    public class ApiResponse
    {
        /// <summary>
        /// Success or Fail
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Message to show
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Data if status == success
        /// </summary>
        public object Data { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MoviesApi.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MoviesApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        public RequestDelegate requestDelegate;

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex, logger);
            }
        }

        private static Task HandleException(HttpContext context, Exception ex, ILogger<ExceptionHandlingMiddleware> logger)
        {
            //var errorMessage = JsonConvert.SerializeObject(new { Message = ex.Message, Code = "GenErr" });
            //context.Response.ContentType = "application/json";

            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //return context.Response.WriteAsync(errorMessage);

            logger.LogError(ex.ToString());

            var errorMessageObject = new Error { Message = ex.Message, Code = "GE" };
            var statusCode = (int)HttpStatusCode.InternalServerError;
            switch (ex)
            {
                case InvalidMovieException:

                    errorMessageObject.Code = "M001";
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
            }
            var errorMessage = JsonConvert.SerializeObject(errorMessageObject);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(errorMessage);
        }
    }
}

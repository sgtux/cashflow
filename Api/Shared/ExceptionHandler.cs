using System;
using System.Net;
using System.Threading.Tasks;
using Cashflow.Api.Service;
using Microsoft.AspNetCore.Http;

namespace Cashflow.Api.Shared
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, DatabaseContext databaseContext, LogService logService)
        {
            try
            {
                await _next(context);
                databaseContext.Commit();
            }
            catch (Exception ex)
            {
                databaseContext.Rollback();
                logService.Error(ex.ToString());
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string result = null;
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            // if (exception is ValidateException)
            // {
            //     statusCode = HttpStatusCode.BadRequest;
            //     result = JsonConvert.SerializeObject(new { error = exception.Message });
            // }
            // else
            //     result = JsonConvert.SerializeObject(new { exception });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cashflow.Api.Shared
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;
        private readonly DatabaseContext databaseContext;

        public ExceptionHandler(RequestDelegate next, DatabaseContext databaseContext)
        {
            this.next = next;
            this.databaseContext = databaseContext;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
                databaseContext.Commit();
            }
            catch (Exception ex)
            {
                databaseContext.Rollback();
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
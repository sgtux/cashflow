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

        private readonly DatabaseContext _databaseContext;

        private readonly LogService _logService;

        public ExceptionHandler(RequestDelegate next, DatabaseContext databaseContext, LogService logService)
        {
            _next = next;
            _databaseContext = databaseContext;
            _logService = logService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                _databaseContext.Commit();
            }
            catch (Exception ex)
            {
                _databaseContext.Rollback();
                _logService.Error(ex.ToString());
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
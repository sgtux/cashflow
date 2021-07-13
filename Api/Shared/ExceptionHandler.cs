using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
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
                var error = ex.ToString();
                Debug.WriteLine(error);
                Console.Write(error);
                databaseContext.Rollback();
                logService.Error(error);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = new { Errors = new[] { "Erro interno no servidor, procure o administrador do sistema." } };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
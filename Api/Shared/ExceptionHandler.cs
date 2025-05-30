using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Http;

namespace Cashflow.Api.Shared
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        private readonly Stopwatch _stopWatch;

        public ExceptionHandler(RequestDelegate next)
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            _next = next;
        }

        public async Task Invoke(HttpContext context, IDatabaseContext databaseContext, LogService logService, IAppConfig appConfig)
        {
            string status = "Success";
            try
            {
                if (appConfig.IsDevelopment)
                    Thread.Sleep(500);
                await _next(context);
                databaseContext.Commit();
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                status = "Error";
                Debug.WriteLine(error);
                Console.Write(error);
                databaseContext.Rollback();
                logService.Error(error);
                await HandleExceptionAsync(context, ex, appConfig, error);
            }
            finally
            {
                var log = $"Path: {context.Request.Path} - Finished in {_stopWatch.ElapsedMilliseconds}ms with {status}";
                logService.Info(log);
                Debug.WriteLine(log);
                Console.WriteLine(log);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IAppConfig appConfig, string error)
        {
            var serverErrorMessage = "Erro interno no servidor, procure o administrador do sistema.";
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (appConfig.IsDevelopment)
                return context.Response.WriteAsync(JsonSerializer.Serialize(new { exception = exception.ToString(), errors = new[] { serverErrorMessage } }));
            else
                return context.Response.WriteAsync(JsonSerializer.Serialize(new { errors = new[] { serverErrorMessage } }));
        }
    }
}
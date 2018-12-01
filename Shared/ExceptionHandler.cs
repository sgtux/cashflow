using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace FinanceApi.Shared
{
  public class ExceptionHandler
  {
    private readonly RequestDelegate next;

    public ExceptionHandler(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      string result = null;
      HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

      if (exception is ValidateException)
        statusCode = HttpStatusCode.BadRequest;
      result = JsonConvert.SerializeObject(new { error = exception.Message });

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)statusCode;
      return context.Response.WriteAsync(result);
    }
  }
}
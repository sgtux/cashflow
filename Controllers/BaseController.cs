using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using FinanceApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
  public abstract class BaseController : Controller
  {
    protected int UserId
    {
      get
      {
        var userId = HttpContext.User.Claims.First(p => p.Type == ClaimTypes.Sid).Value;
        return Convert.ToInt32(userId);
      }
    }

    protected void ThrowValidationError(string error) => throw new ValidateException(error);
  }
}
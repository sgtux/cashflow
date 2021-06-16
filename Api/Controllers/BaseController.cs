using System;
using System.Linq;
using System.Security.Claims;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
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

        protected IActionResult HandleResult(ResultModel result)
        {
            if (result.IsValid)
                return Ok(result);
            else
                return BadRequest(new { Errors = result.Notifications });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Cashflow.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        private Stopwatch _stopWatch;

        public BaseController()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        protected int UserId
        {
            get
            {
                var userId = HttpContext.User.Claims.First(p => p.Type == ClaimTypes.Sid).Value;
                return Convert.ToInt32(userId);
            }
        }

        protected IActionResult HandleResult(ResultModel result)
        {
            if (result.IsValid)
            {
                result.RequestElapsedTime = _stopWatch.ElapsedMilliseconds;
                return Ok(result);
            }
            else
                return BadRequest(new { Errors = result.Notifications, RequestElapsedTime = _stopWatch.ElapsedMilliseconds });
        }

        protected IActionResult HandleUnauthorized(string text)
        {
            var errors = new List<string>() { text };
            return Unauthorized(new { Errors = errors, RequestElapsedTime = _stopWatch.ElapsedMilliseconds });
        }

        protected IActionResult HandleUnprocessableEntity()
        {
            var errors = new List<string>() { "Não foi possível processar a requisição" };
            return UnprocessableEntity(new { Errors = errors, RequestElapsedTime = _stopWatch.ElapsedMilliseconds });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Cashflow.Api.Auth;
using Cashflow.Api.Models;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
  [Route("api/[controller]")]
  public class AccountController : BaseController
  {
    private AccountService _service;

    private AppConfig _config;

    public AccountController(AccountService service, AppConfig config)
    {
      _config = config;
      _service = service;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Get() => Ok(_service.GetById(UserId));

    [HttpPost]
    public IActionResult Post([FromBody]AccountModel model)
    {
      var user = _service.Add(model);

      var claims = new Dictionary<string, string>();
      claims.Add(ClaimTypes.Sid, user.Id.ToString());

      var token = new TokenModel()
      {
        Token = new JwtTokenBuilder(_config.JwtKey, claims).Build().Value
      };

      return Ok(token);
    }
  }
}
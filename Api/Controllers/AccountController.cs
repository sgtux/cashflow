using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Cashflow.Api.Auth;
using Cashflow.Api.Service;
using FinanceApi.Infra;
using FinanceApi.Infra.Entity;
using FinanceApi.Models;
using FinanceApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cashflow.Api.Controllers
{
  /// Account controller
  [Route("api/[controller]")]
  public class AccountController : BaseController
  {
    private AccountService _service;

    private AppConfiguration _config;

    /// Constructor
    public AccountController(AccountService service, AppConfiguration config)
    {
      _config = config;
      _service = service;
    }

    /// Get user details
    [Authorize]
    [HttpGet]
    public IActionResult Get() => Ok(_service.GetById(UserId));

    /// Create new Account
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
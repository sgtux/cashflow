using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Cashflow.Api.Auth;
using Cashflow.Api.Infra.Entity;
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
    public async Task<IActionResult> Get() => Ok(await _service.GetById(UserId));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]AccountModel model)
    {
      if (model is null)
        return UnprocessableEntity();
      var user = await _service.Add(model.Map<AccountModel, User>());
      if (user.IsValid)
      {
        var claims = new Dictionary<string, string>();
        claims.Add(ClaimTypes.Sid, user.Id.ToString());

        var token = new TokenModel()
        {
          Token = new JwtTokenBuilder(_config.JwtKey, claims).Build().Value
        };
        return Ok(token);
      }
      return HandleResult(user);
    }
  }
}
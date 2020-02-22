using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cashflow.Api.Auth;
using Cashflow.Api.Infra;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Models;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
  /// <summary>
  /// Token
  /// </summary>
  [Route("api/token")]
  public class TokenController : Controller
  {
    private AppConfig _config;

    private AccountService _accountService;

    /// <summary>
    /// Constructor
    /// </summary>
    public TokenController(AccountService accountService, AppConfig config)
    {
      _accountService = accountService;
      _config = config;
    }

    /// <summary>
    /// Obter o token
    /// </summary>
    /// <response code="500">Erro interno no servidor</response>
    /// <response code="401">Não autorizado</response>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]LoginModel model)
    {
      if (model is null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        return Unauthorized();

      var user = await _accountService.Login(model.Email, model.Password);
      if (user == null || user.Password != Utils.Sha1(model.Password))
        return Unauthorized();

      var claims = new Dictionary<string, string>();
      claims.Add(ClaimTypes.Sid, user.Id.ToString());
      var token = new JwtTokenBuilder(_config.JwtKey, claims).Build();
      return Ok(new { token = token.Value });
    }
  }
}
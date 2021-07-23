using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Cashflow.Api.Auth;
using Cashflow.Api.Models;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Route("api/token")]
    public class TokenController : BaseController
    {
        private AppConfig _config;

        private AccountService _accountService;

        public TokenController(AccountService accountService, AppConfig config)
        {
            _accountService = accountService;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginModel model)
        {
            if (model is null || string.IsNullOrEmpty(model.NickName) || string.IsNullOrEmpty(model.Password))
                return HandleUnauthorized("Usuário ou senha inválidos.");

            var user = await _accountService.Login(model.NickName, model.Password);
            if (user == null)
                return HandleUnauthorized("Usuário ou senha inválidos.");

            var claims = new Dictionary<string, string>();
            claims.Add(ClaimTypes.Sid, user.Id.ToString());
            var token = new JwtTokenBuilder(_config.SecretJwtKey, _config.CookieExpiresInMinutes, claims).Build();
            return Ok(new { token = token.Value });
        }
    }
}
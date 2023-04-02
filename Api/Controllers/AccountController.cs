using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Cashflow.Api.Auth;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Services;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly AccountService _service;

        private readonly IAppConfig _config;

        public AccountController(AccountService service, IAppConfig config)
        {
            _config = config;
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get() => HandleResult(await _service.GetById(UserId));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AccountModel model)
        {
            if (model is null)
                return HandleUnprocessableEntity();
            var result = await _service.Add(model.Map<AccountModel, User>());
            if (result.IsValid)
            {
                var claims = new Dictionary<string, string>();
                claims.Add(ClaimTypes.Sid, result.Data.Id.ToString());

                var token = new AccountResultModel()
                {
                    Id = result.Data.Id,
                    Email = result.Data.Email,
                    Token = new JwtTokenBuilder(_config.SecretJwtKey, _config.CookieExpiresInMinutes, claims).Build().Value
                };
                return HandleResult(new ResultDataModel<AccountResultModel>() { Data = token });
            }
            return HandleResult(result);
        }

        [HttpPut("SpendingCeiling")]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            if (user is null)
                return HandleUnprocessableEntity();
            return HandleResult(await _service.UpdateSpendingCeiling(UserId, user.SpendingCeiling));
        }
    }
}
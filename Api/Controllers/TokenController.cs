using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cashflow.Api.Auth;
using Cashflow.Api.Contracts;
using Cashflow.Api.Models;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Route("api/token")]
    public class TokenController : BaseController
    {
        private IAppConfig _config;

        private AccountService _accountService;

        private RemainingBalanceService _remainingBalanceService;

        public TokenController(IAppConfig config,
            AccountService accountService,
            PaymentService paymentService,
            RemainingBalanceService remainingBalanceService,
            LogService logService)
        {
            _config = config;
            _accountService = accountService;
            _remainingBalanceService = remainingBalanceService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginModel model)
        {
            if (model is null)
                return HandleUnprocessableEntity();

            var result = await _accountService.Login(model.NickName, model.Password);

            if (!result.IsValid)
                return HandleUnauthorized(result.Notifications.First());

            await _remainingBalanceService.Recalculate(result.Data.Id, DateTime.Now);

            var claims = new Dictionary<string, string>();
            claims.Add(ClaimTypes.Sid, result.Data.Id.ToString());

            var token = new AccountResultModel()
            {
                Id = result.Data.Id,
                NickName = result.Data.NickName,
                Token = new JwtTokenBuilder(_config.SecretJwtKey, _config.CookieExpiresInMinutes, claims).Build().Value
            };
            return Ok(new ResultDataModel<AccountResultModel>() { Data = token });
        }
    }
}
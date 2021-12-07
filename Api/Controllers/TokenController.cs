using System;
using System.Collections.Generic;
using System.Linq;
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

        private PaymentService _paymentService;

        private RemainingBalanceService _remainingBalanceService;

        public TokenController(AppConfig config,
            AccountService accountService,
            PaymentService paymentService,
            RemainingBalanceService remainingBalanceService)
        {
            _config = config;
            _accountService = accountService;
            _paymentService = paymentService;
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

            await _paymentService.UpdateMonthlyPayments(result.Data.Id);
            await _remainingBalanceService.Update(result.Data.Id);

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
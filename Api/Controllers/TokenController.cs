using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cashflow.Api.Auth;
using Cashflow.Api.Contracts;
using Cashflow.Api.Models;
using Cashflow.Api.Models.Account;
using Cashflow.Api.Services;
using Cashflow.Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Route("api/token")]
    public class TokenController : BaseController
    {
        private readonly IAppConfig _config;

        private readonly AccountService _accountService;

        private readonly RemainingBalanceService _remainingBalanceService;

        public TokenController(IAppConfig config,
            AccountService accountService,
            RemainingBalanceService remainingBalanceService)
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

            var result = await _accountService.Login(model.Email, model.Password);

            if (!result.IsValid)
                return HandleUnauthorized(result.Notifications.First());

            await _remainingBalanceService.Recalculate(result.Data.Id, DateTimeUtils.CurrentDate.AddMonths(-1));

            var claims = new Dictionary<string, string>
            {
                { ClaimTypes.Sid, result.Data.Id.ToString() }
            };

            var token = new AccountResultModel()
            {
                Id = result.Data.Id,
                Email = result.Data.Email,
                Token = new JwtTokenBuilder(_config.SecretJwtKey, _config.CookieExpiresInMinutes, claims).Build().Value
            };
            return Ok(new ResultDataModel<AccountResultModel>() { Data = token });
        }
    }
}
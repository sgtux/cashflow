using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Cashflow.Api.Auth;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Services;
using Cashflow.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly AccountService _service;

        private readonly IAppConfig _config;

        private readonly LogService _logService;

        private readonly RemainingBalanceService _remainingBalanceService;

        public AccountController(AccountService service, IAppConfig config, LogService logService, RemainingBalanceService remainingBalanceService)
        {
            _config = config;
            _service = service;
            _logService = logService;
            _remainingBalanceService = remainingBalanceService;
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
                return HandleResult(new ResultDataModel<AccountResultModel>() { Data = token });
            }
            return HandleResult(result);
        }

        [Authorize]
        [HttpPut("SpendingCeiling")]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            if (user is null)
                return HandleUnprocessableEntity();
            return HandleResult(await _service.UpdateSpendingCeiling(UserId, user.SpendingCeiling));
        }


        [HttpGet("GoogleClientId")]
        public IActionResult GetGoogleClientId() => HandleResult(new ResultDataModel<string>(_config.GoogleClientId));

        [HttpPost("GoogleSignIn")]
        public async Task<IActionResult> Post([FromBody] string googleToken)
        {
            if (googleToken == null)
                return HandleUnprocessableEntity();

            using (HttpClient client = new HttpClient())
            {
                string url = $"{_config.GoogleOauthUrl}/tokeninfo?id_token={googleToken}";
                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logService.Error($"Google Oauth Error - {CryptographyUtils.GenerateSHA512(googleToken)} - {responseBody}");
                    ResultModel resultError = new ResultModel();
                    resultError.AddNotification("Erro ao validar o token.");
                    return HandleResult(resultError);
                }

                var googleUserModel = JsonSerializer.Deserialize<GoogleUserModel>(responseBody);

                var result = await _service.Login(googleUserModel);
                if (!result.IsValid)
                    return HandleResult(result);

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
}
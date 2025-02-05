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
using Cashflow.Api.Models.Account;
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
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            if (user is null)
                return HandleUnprocessableEntity();
            user.Id = UserId;
            return HandleResult(await _service.Update(user));
        }


        [HttpGet("GoogleClientId")]
        public IActionResult GetGoogleClientId() => HandleResult(new ResultDataModel<string>(_config.GoogleClientId));

        [HttpPost("GoogleSignIn")]
        public async Task<IActionResult> Post([FromBody] LoginGoogleModel loginModel)
        {
            if (loginModel == null || !loginModel.IsValid)
                return HandleUnprocessableEntity();

            GoogleUserModel googleUserModel = null;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = null;
                if (!string.IsNullOrEmpty(loginModel.IdToken))
                {
                    response = await client.GetAsync($"{_config.GoogleOauthIdTokenUrl}?id_token={loginModel.IdToken}");
                }
                else
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {loginModel.AccessToken}");
                    response = await client.GetAsync($"{_config.GoogleOauthAccessTokenUrl}");
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var tokenType = string.IsNullOrEmpty(loginModel.AccessToken) ? "IdToken" : "AccessToken";
                    _logService.Error($"Google Oauth Error - {tokenType} - {responseBody}");
                    ResultModel resultError = new ResultModel();
                    resultError.AddNotification("Erro ao validar o token.");
                    return HandleResult(resultError);
                }
                googleUserModel = JsonSerializer.Deserialize<GoogleUserModel>(responseBody);
            }


            var result = await _service.Login(googleUserModel);
            if (!result.IsValid)
                return HandleResult(result);

            await _remainingBalanceService.Recalculate(result.Data.Id, DateTimeUtils.CurrentDate.AddMonths(-1));

            var claims = new Dictionary<string, string>
                {
                    { ClaimTypes.Sid, result.Data.Id.ToString() }
                };

            var accountResultModel = new AccountResultModel()
            {
                Id = result.Data.Id,
                Email = result.Data.Email,
                Token = new JwtTokenBuilder(_config.SecretJwtKey, _config.CookieExpiresInMinutes, claims).Build().Value,
                Picture = googleUserModel.picture
            };
            return Ok(new ResultDataModel<AccountResultModel>() { Data = accountResultModel });
        }
    }
}
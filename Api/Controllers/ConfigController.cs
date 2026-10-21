using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Route("api/[controller]")]
    public class ConfigController(IAppConfig config) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = new ConfigModel()
            {
                GoogleClientId = config.GoogleClientId,
                AuthApiUrl = config.AuthApiUrl,
                MicrosoftClientId = config.MicrosoftClientId,
                MicrosoftTenantId = config.MicrosoftTenantId,
                MicrosoftRedirectUri = config.MicrosoftRedirectUri,
                MicrosoftAuthority = $"https://login.microsoftonline.com/{config.MicrosoftTenantId}",
            };
            return HandleResult(new ResultDataModel<object>(result));
        }
    }
}
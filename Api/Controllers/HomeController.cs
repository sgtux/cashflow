using System.Threading.Tasks;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class HomeController : BaseController
    {
        private readonly HomeService _homeService;

        public HomeController(HomeService homeService) => _homeService = homeService;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int month, [FromQuery] int year) => HandleResult(await _homeService.GetInfo(UserId, month, year));
    }
}
using System.Threading.Tasks;
using Cashflow.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class HomeController : BaseController
    {
        private PaymentService _service;

        public HomeController(PaymentService service) => _service = service;

        [HttpGet("chart")]
        public async Task<IActionResult> GetChart([FromQuery] int month, [FromQuery] int year) => HandleResult(await _service.GetHomeChart(UserId, month, year));
    }
}
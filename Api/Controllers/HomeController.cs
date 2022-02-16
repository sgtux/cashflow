using System;
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
        private PaymentService _paymentService;

        private RemainingBalanceService _remainingBalanceService;

        private ProjectionService _projectionService;

        public HomeController(PaymentService paymentService, RemainingBalanceService remainingBalanceService, ProjectionService projectionService)
        {
            _paymentService = paymentService;
            _remainingBalanceService = remainingBalanceService;
            _projectionService = projectionService;
        }

        [HttpGet("Chart")]
        public async Task<IActionResult> GetChart([FromQuery] int month, [FromQuery] int year) => HandleResult(await _paymentService.GetHomeChart(UserId, month, year));


        [HttpGet("Projection")]
        public async Task<IActionResult> GetProjection([FromQuery] int month, [FromQuery] int year)
        {
            await _remainingBalanceService.Recalculate(UserId, DateTime.Now);
            return HandleResult(await _projectionService.GetProjection(UserId, month, year));
        }
    }
}
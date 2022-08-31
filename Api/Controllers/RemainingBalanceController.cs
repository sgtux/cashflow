using System;
using System.Threading.Tasks;
using Cashflow.Api.Models;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RemainingBalanceController : BaseController
    {
        private readonly RemainingBalanceService _remainingBalanceService;

        public RemainingBalanceController(RemainingBalanceService remainingBalanceService)
        {
            _remainingBalanceService = remainingBalanceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => HandleResult(await _remainingBalanceService.GetAll(UserId));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RemainingBalanceModel model) => HandleResult(await _remainingBalanceService.Update(UserId, model));

        [HttpPut("Recalculate")]
        public async Task<IActionResult> Recalculate([FromBody] RemainingBalanceModel model) => HandleResult(await _remainingBalanceService.Recalculate(UserId, new DateTime(model.Year, model.Month, 1), true));
    }
}
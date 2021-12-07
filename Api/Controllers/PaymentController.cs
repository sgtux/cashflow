using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentController : BaseController
    {
        private PaymentService _service;

        private RemainingBalanceService _remainingBalanceService;

        public PaymentController(PaymentService service, RemainingBalanceService remainingBalanceService)
        {
            _service = service;
            _remainingBalanceService = remainingBalanceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaymentFilterModel filter) => HandleResult(await _service.GetByUser(UserId, filter));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => HandleResult(await _service.Get(id, UserId));

        [HttpGet("Types")]
        public async Task<IActionResult> GetTypes() => HandleResult(await _service.GetTypes());

        [HttpGet("Projection")]
        public async Task<IActionResult> GetProjection([FromQuery] int month, [FromQuery] int year)
        {
            await _remainingBalanceService.Update(UserId);
            return HandleResult(await _service.GetProjection(UserId, month, year));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Payment payment)
        {
            if (payment is null)
                return HandleUnprocessableEntity();
            payment.UserId = UserId;
            payment.Id = 0;
            return HandleResult(await _service.Add(payment));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Payment payment)
        {
            if (payment is null)
                return HandleUnprocessableEntity();
            payment.UserId = UserId;
            return HandleResult(await _service.Update(payment));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
    }
}
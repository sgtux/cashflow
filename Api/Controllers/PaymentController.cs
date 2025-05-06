using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentController : BaseController
    {
        private readonly PaymentService _service;

        public PaymentController(PaymentService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaymentFilter filter) => HandleResult(await _service.GetByUser(UserId, filter));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => HandleResult(await _service.Get(id, UserId));

        [HttpGet("Types")]
        public IActionResult GetTypes() => HandleResult(_service.GetTypes());

        [HttpGet("GenerateInstallments")]
        public async Task<IActionResult> GenerateInstallments([FromQuery] GenerateInstallmentsModel model) => HandleResult(await _service.GenerateInstallments(model, UserId));

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
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models.CreditCard;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CreditCardController : BaseController
    {
        private readonly CreditCardService _service;

        public CreditCardController(CreditCardService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() => HandleResult(await _service.GetByUser(UserId));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreditCardEntity card)
        {
            if (card is null)
                return HandleUnprocessableEntity();
            card.UserId = UserId;
            return HandleResult(await _service.Add(card));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CreditCardEntity card)
        {
            if (card is null)
                return HandleUnprocessableEntity();
            card.UserId = UserId;
            return HandleResult(await _service.Update(card));
        }

        [HttpPut("PayCurrentInvoicePayment")]
        public async Task<IActionResult> PutPayCurrentInvoicePayment([FromBody] PayCurrentInvoicePaymentModel model)
        {
            if (model is null)
                return HandleUnprocessableEntity();
            return HandleResult(await _service.PayCurrentInvoicePayment(model, UserId));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
    }
}
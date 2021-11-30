using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CreditCardController : BaseController
    {
        private CreditCardService _service;

        public CreditCardController(CreditCardService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() => HandleResult(await _service.GetByUser(UserId));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreditCard card)
        {
            if (card is null)
                return HandleUnprocessableEntity();
            card.UserId = UserId;
            return HandleResult(await _service.Add(card));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CreditCard card)
        {
            if (card is null)
                return HandleUnprocessableEntity();
            card.UserId = UserId;
            return HandleResult(await _service.Update(card));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
    }
}
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FuelExpenseController : BaseController
    {
        private readonly FuelExpenseService _service;

        public FuelExpenseController(FuelExpenseService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FuelExpense fuelExpense)
        {
            if (fuelExpense is null)
                return UnprocessableEntity();
            return HandleResult(await _service.Add(fuelExpense, UserId));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FuelExpense fuelExpense)
        {
            if (fuelExpense is null)
                return UnprocessableEntity();
            fuelExpense.Id = id;
            return HandleResult(await _service.Update(fuelExpense, UserId));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
    }
}
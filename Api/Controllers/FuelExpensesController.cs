using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FuelExpensesController : BaseController
    {
        private readonly FuelExpensesService _service;

        public FuelExpensesController(FuelExpensesService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FuelExpenses fuelExpenses)
        {
            if (fuelExpenses is null)
                return UnprocessableEntity();
            return HandleResult(await _service.Add(fuelExpenses, UserId));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FuelExpenses fuelExpenses)
        {
            if (fuelExpenses is null)
                return UnprocessableEntity();
            fuelExpenses.Id = id;
            return HandleResult(await _service.Update(fuelExpenses, UserId));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
    }
}
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FuelExpensesController : BaseController
    {
        private FuelExpensesService _service;

        public FuelExpensesController(FuelExpensesService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FuelExpenses fuelExpenses)
        {
            if (fuelExpenses is null)
                return UnprocessableEntity();
            return HandleResult(await _service.Add(fuelExpenses, UserId));
        }
    }
}
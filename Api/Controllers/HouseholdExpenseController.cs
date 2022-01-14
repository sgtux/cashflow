using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class HouseholdExpenseController : BaseController
    {
        private HouseholdExpenseService _service;

        public HouseholdExpenseController(HouseholdExpenseService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetByUser([FromQuery] int month, [FromQuery] int year) => HandleResult(await _service.GetByUser(UserId, month, year));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id) => HandleResult(await _service.GetById(id, UserId));

        [HttpGet("Types")]
        public IActionResult GetTypes() => HandleResult(_service.GetTypes());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HouseholdExpense householdExpense)
        {
            if (householdExpense is null)
                return HandleUnprocessableEntity();
            householdExpense.UserId = UserId;
            return HandleResult(await _service.Add(householdExpense));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] HouseholdExpense householdExpense)
        {
            if (householdExpense is null)
                return HandleUnprocessableEntity();
            householdExpense.UserId = UserId;
            return HandleResult(await _service.Update(householdExpense));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
    }
}
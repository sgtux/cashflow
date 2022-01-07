using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RecurringExpenseController : BaseController
    {
        private RecurringExpenseService _service;

        public RecurringExpenseController(RecurringExpenseService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetByUser() => HandleResult(await _service.GetByUser(UserId));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => HandleResult(await _service.GetById(id, UserId));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RecurringExpense expense)
        {
            if (expense is null)
                return HandleUnprocessableEntity();
            expense.UserId = UserId;
            return HandleResult(await _service.Add(expense));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RecurringExpense expense)
        {
            if (expense is null)
                return HandleUnprocessableEntity();
            expense.UserId = UserId;
            expense.Id = id;
            return HandleResult(await _service.Update(expense));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));

        [HttpPost("History")]
        public async Task<IActionResult> PostHistory([FromBody] RecurringExpenseHistory history)
        {
            if (history is null)
                return HandleUnprocessableEntity();
            return HandleResult(await _service.AddHistory(history, UserId));
        }

        [HttpPut("History/{id}")]
        public async Task<IActionResult> PutHistory(int id, [FromBody] RecurringExpenseHistory history)
        {
            if (history is null)
                return HandleUnprocessableEntity();
            history.Id = id;
            return HandleResult(await _service.UpdateHistory(history, UserId));
        }

        [HttpDelete("{recurringExpenseId}/History/{id}")]
        public async Task<IActionResult> DeleteHistory(int id, int recurringExpenseId) => HandleResult(await _service.RemoveHistory(id, recurringExpenseId, UserId));
    }
}
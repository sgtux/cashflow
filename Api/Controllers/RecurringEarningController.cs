using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RecurringEarningController : BaseController
    {
        private readonly RecurringEarningService _service;

        public RecurringEarningController(RecurringEarningService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetByUser([FromQuery] byte? active) => HandleResult(await _service.GetByUser(UserId, active));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => HandleResult(await _service.GetById(id, UserId));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RecurringEarningEntity earning)
        {
            if (earning is null)
                return HandleUnprocessableEntity();
            earning.UserId = UserId;
            return HandleResult(await _service.Add(earning));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RecurringEarningEntity earning)
        {
            if (earning is null)
                return HandleUnprocessableEntity();
            earning.UserId = UserId;
            earning.Id = id;
            return HandleResult(await _service.Update(earning));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));

        [HttpPost("History")]
        public async Task<IActionResult> PostHistory([FromBody] RecurringEarningHistoryEntity history)
        {
            if (history is null)
                return HandleUnprocessableEntity();
            return HandleResult(await _service.AddHistory(history, UserId));
        }

        [HttpPut("History/{id}")]
        public async Task<IActionResult> PutHistory(int id, [FromBody] RecurringEarningHistoryEntity history)
        {
            if (history is null)
                return HandleUnprocessableEntity();
            history.Id = id;
            return HandleResult(await _service.UpdateHistory(history, UserId));
        }

        [HttpDelete("{recurringEarningId}/History/{id}")]
        public async Task<IActionResult> DeleteHistory(int id, int recurringEarningId) => HandleResult(await _service.RemoveHistory(id, recurringEarningId, UserId));
    }
}

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
    public class DailyExpensesController : BaseController
    {
        private DailyExpensesService _service;

        public DailyExpensesController(DailyExpensesService service) => _service = service;

        [HttpGet]
        public async Task<ResultDataModel<IEnumerable<DailyExpenses>>> GetByUser() => await _service.GetByUser(UserId);

        [HttpGet("{id}")]
        public async Task<ResultDataModel<DailyExpenses>> Get(long id) => await _service.GetById(id, UserId);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DailyExpenses dailyExpenses)
        {
            if (dailyExpenses is null)
                return UnprocessableEntity();
            dailyExpenses.UserId = UserId;
            return HandleResult(await _service.Add(dailyExpenses));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DailyExpenses dailyExpenses)
        {
            if (dailyExpenses is null)
                return UnprocessableEntity();
            dailyExpenses.UserId = UserId;
            return HandleResult(await _service.Update(dailyExpenses));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
    }
}
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SalaryController : BaseController
    {
        private SalaryService _service;

        public SalaryController(SalaryService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() => HandleResult(await _service.GetByUser(UserId));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Salary salary)
        {
            if (salary is null)
                return UnprocessableEntity();
            salary.UserId = UserId;
            return HandleResult(await _service.Add(salary));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Salary salary)
        {
            if (salary is null)
                return UnprocessableEntity();
            salary.UserId = UserId;
            return HandleResult(await _service.Update(salary));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
    }
}
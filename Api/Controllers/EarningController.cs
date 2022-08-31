using System;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EarningController : BaseController
    {
        private readonly EarningService _service;

        public EarningController(EarningService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime? fromDate) => HandleResult(await _service.GetByUser(UserId, fromDate));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => HandleResult(await _service.GetById(id));

        [HttpGet("Types")]
        public IActionResult GetTypes() => HandleResult(_service.GetTypes());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Earning earning)
        {
            if (earning is null)
                return HandleUnprocessableEntity();
            earning.UserId = UserId;
            return HandleResult(await _service.Add(earning));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Earning earning)
        {
            if (earning is null)
                return HandleUnprocessableEntity();
            earning.UserId = UserId;
            return HandleResult(await _service.Update(earning));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
    }
}
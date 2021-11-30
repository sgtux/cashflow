using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class VehicleController : BaseController
    {
        private VehicleService _service;

        public VehicleController(VehicleService service) => _service = service;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => HandleResult(await _service.GetById(id, UserId));

        [HttpGet]
        public async Task<IActionResult> GetByUser(int id) => HandleResult(await _service.GetByUserId(UserId));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Vehicle vehicle)
        {
            if (vehicle is null)
                return HandleUnprocessableEntity();
            vehicle.UserId = UserId;
            return HandleResult(await _service.Add(vehicle));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Vehicle vehicle)
        {
            if (vehicle is null)
                return HandleUnprocessableEntity();
            vehicle.UserId = UserId;
            vehicle.Id = id;
            return HandleResult(await _service.Update(vehicle));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Delete(id, UserId));
    }
}
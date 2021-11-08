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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Vehicle vehicle)
        {
            if (vehicle is null)
                return UnprocessableEntity();
            vehicle.UserId = UserId;
            return HandleResult(await _service.Add(vehicle));
        }
    }
}
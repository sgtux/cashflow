using System.Threading.Tasks;
using Cashflow.Api.Services;
using Cashflow.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProjectionController : BaseController
    {
        private readonly RemainingBalanceService _remainingBalanceService;

        private readonly ProjectionService _projectionService;

        public ProjectionController(RemainingBalanceService remainingBalanceService, ProjectionService projectionService)
        {
            _remainingBalanceService = remainingBalanceService;
            _projectionService = projectionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjection()
        {
            await _remainingBalanceService.Recalculate(UserId, DateTimeUtils.CurrentDate.AddMonths(-1));
            return HandleResult(await _projectionService.GetProjection(UserId));
        }
    }
}
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : BaseController
    {
        private readonly IUserRepository _repository;

        public StatusController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string dbDate = "ERRO";
            try
            {
                dbDate = (await _repository.DbCurrentDate()).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch { }
            var status = new
            {
                DbCurrentDate = dbDate,
                ApiCurrentDate = Utils.CurrentDate.ToString("dd/MM/yyyy HH:mm:ss")
            };
            return HandleResult(new ResultDataModel<object>(status));
        }
    }
}
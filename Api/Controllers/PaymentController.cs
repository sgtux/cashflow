using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
{
  /// <summary>
  /// Pagamentos
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  public class PaymentController : BaseController
  {
    private PaymentService _service;

    /// <summary>
    /// Construtor
    /// </summary>    
    public PaymentController(PaymentService service) => _service = service;

    /// <summary>
    /// Obter os pagamentos usuário logado
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get() => HandleResult(await _service.GetByUser(UserId));

    /// <summary>
    /// Obter os pagamentos usuário logado
    /// </summary>
    /// <returns></returns>
    [Route("FuturePayments")]
    [HttpGet]
    public async Task<Dictionary<string, PaymentFutureResultModel>> GetFuturePayments([FromQuery]DateTime endDate)
    {
      return await _service.GetFuturePayments(UserId, endDate);
    }

    /// <summary>
    /// Inserir um novo pagamento para o usuário logado
    /// </summary>
    /// <param name="payment"></param>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Payment payment)
    {
      if (payment is null)
        return UnprocessableEntity();
      payment.UserId = UserId;
      return HandleResult(await _service.Add(payment));
    }

    /// <summary>
    /// Atualizar um pagamento do usuário logado
    /// </summary>
    /// <param name="payment"></param>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody]Payment payment)
    {
      if (payment is null)
        return UnprocessableEntity();
      payment.UserId = UserId;
      return HandleResult(await _service.Update(payment));
    }

    /// <summary>
    /// Remove um cartão de crédito do usuário logado
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
  }
}
using System;
using System.Collections.Generic;
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
    public IEnumerable<Payment> Get() => _service.GetByUser(UserId);

    /// <summary>
    /// Obter os pagamentos usuário logado
    /// </summary>
    /// <returns></returns>
    [Route("FuturePayments")]
    [HttpGet]
    public Dictionary<string, PaymentFutureResultModel> GetFuturePayments([FromQuery]DateTime forecastAt) => _service.GetFuturePayments(UserId, forecastAt);

    /// <summary>
    /// Inserir um novo pagamento para o usuário logado
    /// </summary>
    /// <param name="payment"></param>
    [HttpPost]
    public void Post([FromBody]Payment payment) => _service.Add(payment, UserId);

    /// <summary>
    /// Atualizar um pagamento do usuário logado
    /// </summary>
    /// <param name="payment"></param>
    [HttpPut]
    public void Put([FromBody]Payment payment) => _service.Update(payment, UserId);

    /// <summary>
    /// Remove um cartão de crédito do usuário logado
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    public void Delete(int id) => _service.Remove(id, UserId);
  }
}
using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Service;
using FinanceApi.Infra.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
  /// <summary>
  /// Cartões de crédito
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  public class CreditCardController : BaseController
  {
    private CreditCardService _service;

    /// <summary>
    /// Construtor
    /// </summary>    
    public CreditCardController(CreditCardService service) => _service = service;

    /// <summary>
    /// Obter os cartões de crédito do usuário logado
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public List<CreditCard> Get() => _service.GetByUser(UserId);

    /// <summary>
    /// Inserir um novo cartão de crédito para o usuário logado
    /// </summary>
    /// <param name="card"></param>
    [HttpPost]
    public void Post([FromBody]CreditCard card) => _service.Add(card.Name, UserId);

    /// <summary>
    /// Atualiza um cartão de crédito do usuário logado
    /// </summary>
    /// <param name="card"></param>
    [HttpPut]
    public void Put([FromBody]CreditCard card) => _service.Update(card);

    /// <summary>
    /// Remove um cartão de crédito do usuário logado
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    public void Delete(int id) => _service.Remove(id, UserId);
  }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Api.Controllers
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
    public async Task<IEnumerable<CreditCard>> Get() => await _service.GetByUser(UserId);

    /// <summary>
    /// Inserir um novo cartão de crédito para o usuário logado
    /// </summary>
    /// <param name="card"></param>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]CreditCard card)
    {
      if (card is null)
        return UnprocessableEntity();
      card.UserId = UserId;
      return HandleResult(await _service.Add(card));
    }

    /// <summary>
    /// Atualiza um cartão de crédito do usuário logado
    /// </summary>
    /// <param name="card"></param>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody]CreditCard card)
    {
      if (card is null)
        return UnprocessableEntity();
        card.UserId = UserId;
      return HandleResult(await _service.Update(card));
    }

    /// <summary>
    /// Remove um cartão de crédito do usuário logado
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => HandleResult(await _service.Remove(id, UserId));
  }
}
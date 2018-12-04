using System.Collections.Generic;
using System.Linq;
using FinanceApi.Infra;
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
    private AppDbContext _context;

    /// <summary>
    /// Construtor
    /// </summary>    
    public CreditCardController(AppDbContext context) => _context = context;

    /// <summary>
    /// Obter os cartões de crédito do usuário logado
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public List<CreditCard> Get()
    {
      return _context.CreditCard.Where(p => p.UserId == UserId).ToList();
    }

    /// <summary>
    /// Inserir um novo cartão de crédito para o usuário logado
    /// </summary>
    /// <param name="card"></param>
    [HttpPost]
    public void Post([FromBody]CreditCard card)
    {
      if (string.IsNullOrEmpty(card.Name))
        ThrowValidationError("O nome do cartão é obrigatório.");
      card.UserId = UserId;
      _context.Add(card);
      _context.SaveChanges();
    }

    /// <summary>
    /// Atualiza um cartão de crédito do usuário logado
    /// </summary>
    /// <param name="card"></param>
    [HttpPut]
    public void Put([FromBody]CreditCard card)
    {
      if (string.IsNullOrEmpty(card.Name))
        ThrowValidationError("O nome do cartão é obrigatório.");

      var dbCard = _context.CreditCard.FirstOrDefault(p => p.Id == card.Id && p.UserId == UserId);
      if (dbCard is null)
        ThrowValidationError("Cartão não localizado.");

      dbCard.Name = card.Name;
      _context.Update(dbCard);
      _context.SaveChanges();
    }

    /// <summary>
    /// Remove um cartão de crédito do usuário logado
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      var card = _context.CreditCard.FirstOrDefault(p => p.Id == id && p.UserId == UserId);
      if (card is null)
        ThrowValidationError("Cartão não localizado.");

      _context.Remove(card);
      _context.SaveChanges();
    }
  }
}
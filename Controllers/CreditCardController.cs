using System.Collections.Generic;
using System.Linq;
using FinanceApi.Infra;
using FinanceApi.Infra.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  public class CreditCardController : BaseController
  {
    private AppDbContext _context;
    public CreditCardController(AppDbContext context) => _context = context;

    [HttpGet]
    public List<CreditCard> Get()
    {
      return _context.CreditCard.Where(p => p.UserId == UserId).ToList();
    }

    [HttpPost]
    public void Post(CreditCard card)
    {
      if (string.IsNullOrEmpty(card.Name))
        ThrowValidationError("O nome do cartão é obrigatório.");
      card.UserId = UserId;
      _context.Add(card);
      _context.SaveChanges();
    }

    [HttpPut]
    public void Put(CreditCard card)
    {
      if (string.IsNullOrEmpty(card.Name))
        ThrowValidationError("O nome do cartão é obrigatório.");

      var dbCard = _context.CreditCard.FirstOrDefault(p => p.Id == card.Id && p.UserId == UserId);
      if (dbCard is null)
        ThrowValidationError("Cartão não localizado.");

      card.UserId = UserId;
      _context.Update(card);
      _context.SaveChanges();
    }
  }
}
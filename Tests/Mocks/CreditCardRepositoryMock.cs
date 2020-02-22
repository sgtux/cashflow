using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;

namespace Cashflow.Tests.Mocks
{
  public class CreditCardRepositoryMock : BaseRepositoryMock, ICreditCardRepository
  {
    public Task Add(CreditCard t) => Task.Run(() => CreditCards.Add(t));

    public Task<bool> Exists(long userId) => Task.Run(() => CreditCards.Any(p => p.Id == userId));

    public Task<IEnumerable<CreditCard>> GetAll() => Task.Run(() => CreditCards.AsEnumerable());

    public Task<CreditCard> GetById(int id) => Task.Run(() => CreditCards.FirstOrDefault(p => p.Id == id));

    public Task<IEnumerable<CreditCard>> GetByUserId(int userId) => Task.Run(() => CreditCards.Where(p => p.UserId == userId).AsEnumerable());

    public Task<bool> HasPayments(int cardId) => Task.Run(() => Payments.Any(p => p.CreditCardId == cardId));

    public Task Remove(int id)
    {
      return Task.Run(() =>
      {
        var card = CreditCards.FirstOrDefault(p => p.Id == id);
        if (card != null)
          CreditCards.Remove(card);
      });
    }

    public Task Update(CreditCard t)
    {
      return Task.Run(() =>
      {
        var card = CreditCards.FirstOrDefault(p => p.Id == t.Id);
        if (card != null)
          card.Name = t.Name;
      });
    }
  }
}
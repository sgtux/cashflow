using System.Collections.Generic;
using FinanceApi.Infra.Entity;

namespace Cashflow.Tests.Mocks
{
  public abstract class BaseRepositoryMock
  {
    private static List<Payment> _payments;
    private static List<CreditCard> _creditCards;
    private static List<User> _users;

    private List<Payment> CreatePaymentsMock()
    {
      return new List<Payment>()
      {
        new Payment()
        {
          UserId = 1,
          CreditCardId = 1
        },
        new Payment()
        {
          UserId = 1,
          CreditCardId = 1,
          Id = 1
        },
      };
    }

    private List<CreditCard> CreateCreditCard()
    {
      var list = new List<CreditCard>();
      list.Add(new CreditCard() { Id = 1, UserId = 1, Name = "Primeiro Cartão" });
      list.Add(new CreditCard() { Id = 2, UserId = 1, Name = "Segundo Cartão" });
      return list;
    }

    private List<User> CreateUserMock()
    {
      var users = new List<User>();
      users.Add(new User() { Id = 1, Email = "primeirouser@mail.com", Name = "Primeiro User" });
      users.Add(new User() { Id = 2, Email = "primeirouser@mail.com", Name = "Primeiro User" });
      return users;
    }

    protected List<Payment> Payments
    {
      get
      {
        if (_payments == null)
          _payments = CreatePaymentsMock();
        return _payments;
      }
    }

    protected List<CreditCard> CreditCards
    {
      get
      {
        if (_creditCards == null)
          _creditCards = CreateCreditCard();
        return _creditCards;
      }
    }

    protected List<User> Users
    {
      get
      {
        if (_users == null)
          _users = CreateUserMock();
        return _users;
      }
    }
  }
}
using System;
using System.Collections.Generic;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Tests.Mocks
{
  public abstract class BaseRepositoryMock
  {
    private static List<Payment> _payments;
    private static List<CreditCard> _creditCards;
    private static List<User> _users;

    private List<Payment> CreatePaymentsMock()
    {
      var list = new List<Payment>();
      list.Add(new Payment() { UserId = 1, CreditCardId = 1 });
      list.Add(new Payment() { Id = 2, UserId = 1, CreditCardId = 1 });
      list.Add(new Payment() { Id = 3, UserId = 1, CreditCardId = 1 });
      list.Add(new Payment() { Id = 4, UserId = 2, CreditCardId = 1 });

      list.Add(new Payment() { Id = 5, UserId = 3, CreditCardId = 1, Type = TypePayment.Expense, FirstPayment = new DateTime(2019, 6, 1), Cost = 544, FixedPayment = true });
      list.Add(new Payment() { Id = 6, UserId = 3, CreditCardId = 1, Type = TypePayment.Expense, FirstPayment = new DateTime(2019, 3, 1), Cost = 423, Plots = 3 });
      list.Add(new Payment() { Id = 7, UserId = 3, CreditCardId = 1, Type = TypePayment.Expense, FirstPayment = new DateTime(2019, 7, 1), Cost = 77, SinglePlot = true });
      list.Add(new Payment() { Id = 11, UserId = 3, CreditCardId = 1, Type = TypePayment.Income, FirstPayment = new DateTime(2019, 7, 1), Cost = 10000, FixedPayment = true });
      list.Add(new Payment() { Id = 13, UserId = 3, CreditCardId = 1, Type = TypePayment.Income, FirstPayment = new DateTime(2019, 3, 1), Cost = 225, Plots = 3 });
      list.Add(new Payment() { Id = 14, UserId = 3, CreditCardId = 1, Type = TypePayment.Income, FirstPayment = new DateTime(2019, 5, 1), Cost = 54, SinglePlot = true });

      return list;
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
      users.Add(new User() { Id = 1, Email = "primeirousuario@mail.com", Name = "Primeiro Usuário" });
      users.Add(new User() { Id = 2, Email = "segundousuario@mail.com", Name = "Segundo Usuário" });
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
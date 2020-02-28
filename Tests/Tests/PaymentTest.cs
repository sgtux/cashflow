using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
using Cashflow.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashflow.Tests
{
  [TestClass]
  public class PaymentTest : BaseTest
  {
    private PaymentService _service;

    [TestInitialize]
    public void Init()
    {
      _service = new PaymentService(new PaymentRepositoryMock(), new CreditCardRepositoryMock());
    }

    private Payment DefaultPayment
    {
      get
      {
        return new Payment()
        {
          Id = 1,
          UserId = 1,
          CreditCardId = 1,
          Description = "First Payment",
          FixedPayment = true,
          Type = TypePayment.Expense,
          Installments = new List<Installment>()
          {
            new Installment() { Number = 1, Id = 1, Cost = 1500, Date = new DateTime(2020, 1, 1) }
          }
        };
      }
    }

    [TestMethod]
    public async Task GetUserPayments()
    {
      var payments = await _service.GetByUser(2) as ResultDataModel<IEnumerable<Payment>>;
      Assert.IsFalse(payments.Data.Any(p => p.UserId != 2));
    }

    // [TestMethod]
    // public async Task GetFuturePaymentsWithoutForecastAt()
    // {
    //   var payments = await _service.GetFuturePayments(3, default(DateTime));

    //   Assert.AreEqual(payments.Count, 12);

    //   Assert.AreEqual(141, payments["05/2019"].CostExpense); // 423/3
    //   Assert.AreEqual(129, payments["05/2019"].CostIncome); // (225/3) + 54
    //   Assert.AreEqual(-12, payments["05/2019"].Total); // 129 - 141

    //   Assert.AreEqual(621, payments["07/2019"].CostExpense); // 544 + 77
    //   Assert.AreEqual(10000, payments["07/2019"].CostIncome); // 10000
    //   Assert.AreEqual(9379, payments["07/2019"].Total); // 10000 - (544 + 77)
    //   Assert.AreEqual(8757, payments["07/2019"].AccumulatedCost); // (10000 + ((225/3)*2) + 54) - ((2 * 544) + ((423/3)*2) + 77)
    // }

    // [TestMethod]
    // public async Task GetFuturePayments()
    // {
    //   var payments = await _service.GetFuturePayments(3, new DateTime(2019, 10, 1));
    //   Assert.AreEqual(37125, payments["10/2019"].AccumulatedCost); // ((10000 * 4) + ((225/3)*2) + 54) - ((5 * 544) + ((423/3)*2) + 77)
    //   Assert.AreEqual(payments.Count, 7);
    // }

    [TestMethod]
    public async Task AddWithNoDescription()
    {
      var p = DefaultPayment;
      p.Description = "";
      var result = await _service.Add(p);
      HasNotifications(result, "'Description' must not be empty.");
    }

    [TestMethod]
    public async Task AddWithNoPlots()
    {
      var p = DefaultPayment;
      p.Installments = new List<Installment>();
      var result = await _service.Add(p);
      HasNotifications(result, "'Installments' must not be empty.");
    }

    [TestMethod]
    public async Task AddWithCreditCardIdNotFound()
    {
      var p = DefaultPayment;
      p.CreditCardId = 99;
      var result = await _service.Add(p);
      HasNotifications(result, "Credit card not found.");
    }

    [TestMethod]
    public async Task AddOK()
    {
      var p = DefaultPayment;
      p.CreditCardId = 1;
      var result = await _service.Add(p);
      HasNotifications(result);
    }

    [TestMethod]
    public async Task UpdateWithNoDescription()
    {
      var p = DefaultPayment;
      p.Description = "";
      var result = await _service.Update(p);
      HasNotifications(result, "'Description' must not be empty.");
    }

    [TestMethod]
    public async Task UpdateWithNoPlots()
    {
      var p = DefaultPayment;
      p.Installments = new List<Installment>();
      var result = await _service.Update(p);
      HasNotifications(result, "'Installments' must not be empty.");
    }

    [TestMethod]
    public async Task UpdatePaymentNotFound()
    {
      var p = DefaultPayment;
      p.Id = 99;
      p.CreditCardId = 1;
      var result = await _service.Update(p);
      HasNotifications(result, "Payment not found.");
    }

    [TestMethod]
    public async Task UpdateWithCreditCardIdNotFound()
    {
      var p = DefaultPayment;
      p.CreditCardId = 99;
      var result = await _service.Update(p);
      HasNotifications(result, "Credit card not found.");
    }

    [TestMethod]
    public async Task UpdateOK()
    {
      var p = DefaultPayment;
      p.CreditCardId = 1;
      var result = await _service.Update(p);
      HasNotifications(result);
    }

    [TestMethod]
    public async Task RemoveWithPaymentNotFound()
    {
      var result = await _service.Remove(99, 1);
      HasNotifications(result, "Payment not found.");
    }

    [TestMethod]
    public async Task RemoveOK()
    {
      var result = await _service.Remove(3, 1);
      HasNotifications(result);
    }
  }
}
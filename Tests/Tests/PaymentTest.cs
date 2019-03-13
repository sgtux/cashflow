using System;
using System.Linq;
using Cashflow.Api.Infra.Entity;
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
          Description = "pagamento",
          UserId = 1,
          Cost = 34,
          Plots = 8,
          Type = TypePayment.Expense,
          FirstPayment = DateTime.Now,
          PlotsPaid = 5,
          FixedPayment = true,
          SinglePlot = false
        };
      }
    }

    [TestMethod]
    public void GetUserPayments()
    {
      var payments = _service.GetByUser(2);
      Assert.IsFalse(payments.Any(p => p.UserId != 2));
    }

    [TestMethod]
    public void GetFuturePaymentsWithoutForecastAt()
    {
      var payments = _service.GetFuturePayments(3, default(DateTime));

      Assert.AreEqual(payments.Count, 12);

      Assert.AreEqual(141, payments["05/2019"].CostExpense); // 423/3
      Assert.AreEqual(129, payments["05/2019"].CostIncome); // (225/3) + 54
      Assert.AreEqual(-12, payments["05/2019"].Cost); // 129 - 141

      Assert.AreEqual(621, payments["07/2019"].CostExpense); // 544 + 77
      Assert.AreEqual(10000, payments["07/2019"].CostIncome); // 10000
      Assert.AreEqual(9379, payments["07/2019"].Cost); // 10000 - (544 + 77)
      Assert.AreEqual(8757, payments["07/2019"].AcumulatedCost); // (10000 + ((225/3)*2) + 54) - ((2 * 544) + ((423/3)*2) + 77)

      Assert.AreEqual(2, payments["04/2019"].Payments.First(p => p.Plots == 3).Items.First().PlotsPaid);
      Assert.AreEqual(3, payments["05/2019"].Payments.First(p => p.Plots == 3).Items.First().PlotsPaid);
    }

    [TestMethod]
    public void GetFuturePayments()
    {
      var payments = _service.GetFuturePayments(3, new DateTime(2019, 10, 1));
      Assert.AreEqual(37125, payments["10/2019"].AcumulatedCost); // ((10000 * 4) + ((225/3)*2) + 54) - ((5 * 544) + ((423/3)*2) + 77)
      Assert.AreEqual(payments.Count, 7);
    }

    [TestMethod]
    public void AddWithInvalidPayment()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        _service.Add(null, p.UserId);
      }, "Pagamento inválido.");
    }

    [TestMethod]
    public void AddWithNoDescription()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.Description = "";
        _service.Add(p, p.UserId);
      }, "A descrição é obrigatória.");
    }

    [TestMethod]
    public void AddWithZeroCost()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.Cost = 0;
        _service.Add(p, p.UserId);
      }, "O valor deve ser maior que Zero.");
    }

    [TestMethod]
    public void AddWithNoFirstPayment()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.FirstPayment = default(DateTime);
        _service.Add(p, p.UserId);
      }, "A data do primeiro pagamento é obrigatória.");
    }

    [TestMethod]
    public void AddWithWrongNumberPlots()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.FixedPayment = false;
        p.SinglePlot = false;
        p.Plots = 8;
        p.PlotsPaid = 10;
        _service.Add(p, p.UserId);
      }, "O quantidade parcelas pagas não pode ser maior que o número de parcelas.");
    }

    [TestMethod]
    public void AddWithNoPlots()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.PlotsPaid = 0;
        p.Plots = 0;
        p.FixedPayment = false;
        p.SinglePlot = false;
        _service.Add(p, p.UserId);
      }, "O pagamento deve ter pelo menos 1 parcela.");
    }

    [TestMethod]
    public void AddWithCreditCardIdNotFound()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.CreditCardId = 99;
        p.CreditCard = null;
        _service.Add(p, p.UserId);
      }, "Cartão não localizado.");
    }

    [TestMethod]
    public void AddWithCreditCardNotFound()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.CreditCardId = 0;
        p.CreditCard = new CreditCard() { Id = 99 };
        _service.Add(p, p.UserId);
      }, "Cartão não localizado.");
    }

    [TestMethod]
    public void AddOK()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.CreditCardId = 1;
        _service.Add(p, p.UserId);
      });
    }

    [TestMethod]
    public void UpdateWithInvalidPayment()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        _service.Update(null, p.UserId);
      }, "Pagamento inválido.");
    }

    [TestMethod]
    public void UpdateWithNoDescription()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.Description = "";
        _service.Update(p, p.UserId);
      }, "A descrição é obrigatória.");
    }

    [TestMethod]
    public void UpdateWithZeroCost()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.Cost = 0;
        _service.Update(p, p.UserId);
      }, "O valor deve ser maior que Zero.");
    }

    [TestMethod]
    public void UpdateWithNoFirstPayment()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.FirstPayment = default(DateTime);
        _service.Update(p, p.UserId);
      }, "A data do primeiro pagamento é obrigatória.");
    }

    [TestMethod]
    public void UpdateWithWrongNumberPlots()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.FixedPayment = false;
        p.SinglePlot = false;
        p.Plots = 8;
        p.PlotsPaid = 10;
        _service.Update(p, p.UserId);
      }, "O quantidade parcelas pagas não pode ser maior que o número de parcelas.");
    }

    [TestMethod]
    public void UpdateWithNoPlots()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.PlotsPaid = 0;
        p.Plots = 0;
        p.FixedPayment = false;
        p.SinglePlot = false;
        _service.Update(p, p.UserId);
      }, "O pagamento deve ter pelo menos 1 parcela.");
    }

    [TestMethod]
    public void UpdatePaymentNotFound()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.Id = 99;
        p.CreditCardId = 1;
        _service.Update(p, p.UserId);
      }, "Pagamento não localizado.");
    }

    [TestMethod]
    public void UpdateWithCreditCardIdNotFound()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.CreditCardId = 99;
        p.CreditCard = null;
        _service.Update(p, p.UserId);
      }, "Cartão não localizado.");
    }

    [TestMethod]
    public void UpdateWithCreditCardNotFound()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.CreditCardId = 0;
        p.CreditCard = new CreditCard() { Id = 99 };
        _service.Update(p, p.UserId);
      }, "Cartão não localizado.");
    }

    [TestMethod]
    public void UpdateOK()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.CreditCardId = 1;
        _service.Update(p, p.UserId);
      });
    }

    [TestMethod]
    public void RemoveWithPaymentNotFound()
    {
      AssertExceptionMessage(() =>
      {
        _service.Remove(99, 1);
      }, "Pagamento não localizado.");
    }

    [TestMethod]
    public void RemoveOK()
    {
      AssertExceptionMessage(() =>
      {
        _service.Remove(3, 1);
      });
    }
  }
}
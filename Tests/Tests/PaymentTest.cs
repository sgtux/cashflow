using System;
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
    public void AddWithInvalidPayment()
    {
      AssertExceptionMessage(() =>
      {
        _service.Add(null);
      }, "Pagamento inválido.");
    }

    [TestMethod]
    public void AddWithNoDescription()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.Description = "";
        _service.Add(p);
      }, "A descrição é obrigatória.");
    }

    [TestMethod]
    public void AddWithZeroCost()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.Cost = 0;
        _service.Add(p);
      }, "O valor deve ser maior que Zero.");
    }

    [TestMethod]
    public void AddWithNoFirstPayment()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.FirstPayment = default(DateTime);
        _service.Add(p);
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
        _service.Add(p);
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
        _service.Add(p);
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
        _service.Add(p);
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
        _service.Add(p);
      }, "Cartão não localizado.");
    }

    [TestMethod]
    public void AddOK()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.CreditCardId = 1;
        _service.Add(p);
      });
    }

    [TestMethod]
    public void GetFuturePayments()
    {
      // var response = Client.GetAsync("/api/Payment/FuturePayments").Result;
      // var statusCode = (int)response.StatusCode;
      // Assert.AreEqual(200, statusCode);
    }

    [TestMethod]
    public void UpdateWithInvalidPayment()
    {
      AssertExceptionMessage(() =>
      {
        _service.Update(null);
      }, "Pagamento inválido.");
    }

    [TestMethod]
    public void UpdateWithNoDescription()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.Description = "";
        _service.Update(p);
      }, "A descrição é obrigatória.");
    }

    [TestMethod]
    public void UpdateWithZeroCost()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.Cost = 0;
        _service.Update(p);
      }, "O valor deve ser maior que Zero.");
    }

    [TestMethod]
    public void UpdateWithNoFirstPayment()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.FirstPayment = default(DateTime);
        _service.Update(p);
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
        _service.Update(p);
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
        _service.Update(p);
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
        _service.Update(p);
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
        _service.Update(p);
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
        _service.Update(p);
      }, "Cartão não localizado.");
    }

    [TestMethod]
    public void UpdateOK()
    {
      AssertExceptionMessage(() =>
      {
        var p = DefaultPayment;
        p.CreditCardId = 1;
        _service.Update(p);
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
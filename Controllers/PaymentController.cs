using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FinanceApi.Infra;
using FinanceApi.Infra.Entity;
using FinanceApi.Models;
using FinanceApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Controllers
{
  /// <summary>
  /// Pagamentos
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  public class PaymentController : BaseController
  {

    private AppDbContext _context;

    /// <summary>
    /// Construtor
    /// </summary>    
    public PaymentController(AppDbContext context) => _context = context;

    /// <summary>
    /// Obter os pagamentos usuário logado
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public List<Payment> Get()
    {
      return _context.Payment.Include(p => p.CreditCard).Where(p => p.UserId == UserId).ToList();
    }

    /// <summary>
    /// Obter os pagamentos usuário logado
    /// </summary>
    /// <returns></returns>
    [Route("FuturePayments")]
    [HttpGet]
    public object GetFuturePayments([FromQuery]DateTime forecastAt)
    {
      var now = DateTime.Now;
      var dates = new List<DateTime>();

      if (forecastAt == default(DateTime) || forecastAt < now)
        forecastAt = now.AddMonths(2);
      else
        forecastAt.AddMonths(1);

      var currentDate = now;
      var months = 0;
      while (forecastAt.Month != currentDate.Month || forecastAt.Year != currentDate.Year)
      {
        currentDate = now.AddMonths(months);
        dates.Add(currentDate);
        months++;
      }

      var result = new Dictionary<string, PaymentFutureResultModel>();
      var payments = _context.Payment.Include(p => p.CreditCard)
        .Where(p => p.UserId == UserId).ToList();

      dates.OrderBy(p => p.Ticks).ToList().ForEach(date =>
      {
        var startDate = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        var paymentsMonth = new List<PaymentFutureModel>();
        payments.ForEach(p =>
        {
          var paymentMonths = GetMonthsFromPayment(p);
          if (paymentMonths.Contains(date.ToString("MM/yyyy")) || (p.FixedPayment && p.FirstPayment < startDate))
          {
            string creditCardName = p.CreditCard?.Name;
            PaymentFutureModel paymentModel = null;
            if (!string.IsNullOrEmpty(creditCardName))
            {
              paymentModel = paymentsMonth.FirstOrDefault(x => x.Description == creditCardName);
              if (paymentModel == null)
              {
                paymentModel = new PaymentFutureModel()
                {
                  Description = creditCardName,
                  Type = TypePayment.Expense,
                  IsCreditCard = true,
                  Month = date.ToString("MM/yyyy")
                };
              }
            }
            else
            {
              paymentModel = new PaymentFutureModel()
              {
                Description = p.Description,
                Type = p.Type,
                IsCreditCard = false,
                Month = date.ToString("MM/yyyy")
              };
            }

            paymentModel.Items.Add(new PaymentItemModel()
            {
              PaymentId = p.Id,
              Description = p.Description,
              Cost = p.FixedPayment ? p.Cost : p.Cost / p.Plots,
              Plots = p.FixedPayment ? 0 : p.Plots,
              Type = p.Type,
              CreditCard = creditCardName,
              PaymentDate = date.ToString("dd/MM/yyyy"),
              Month = date.ToString("MM/yyyy"),
              Day = p.FirstPayment.Day
            });
            if (!paymentModel.IsCreditCard
              || (paymentModel.IsCreditCard && !paymentsMonth.Any(x => x.Description == paymentModel.Description)))
              paymentsMonth.Add(paymentModel);
          }
        });
        var resultModel = new PaymentFutureResultModel();
        resultModel.Payments = paymentsMonth;
        resultModel.Cost = paymentsMonth.Sum(p => p.Type == TypePayment.Income ? p.Cost : (p.Cost * -1));
        result.Add(date.ToString("MM/yyyy"), resultModel);
      });

      return result;
    }

    /// <summary>
    /// Inserir um novo pagamento para o usuário logado
    /// </summary>
    /// <param name="payment"></param>
    [HttpPost]
    public void Post([FromBody]Payment payment)
    {
      ValidatePayment(payment);
      payment.UserId = UserId;
      _context.Add(payment);
      _context.SaveChanges();
    }

    /// <summary>
    /// Atualizar um pagamento do usuário logado
    /// </summary>
    /// <param name="payment"></param>
    [HttpPut]
    public void Put([FromBody]Payment payment)
    {
      ValidatePayment(payment);
      var paymentDb = _context.Payment.FirstOrDefault(p => p.Id == payment.Id && p.UserId == UserId);
      if (paymentDb is null)
        ThrowValidationError("Pagamento não localizado.");
      payment.UserId = UserId;
      payment.MapperTo(paymentDb);
      _context.Update(paymentDb);
      _context.SaveChanges();
    }

    /// <summary>
    /// Remove um cartão de crédito do usuário logado
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      var payment = _context.Payment.FirstOrDefault(p => p.Id == id && p.UserId == UserId);
      if (payment is null)
        ThrowValidationError("Pagamento não localizado.");

      _context.Remove(payment);
      _context.SaveChanges();
    }

    private void ValidatePayment(Payment payment)
    {
      if (payment is null)
        ThrowValidationError("Pagamento inválido.");

      if (string.IsNullOrEmpty(payment.Description))
        ThrowValidationError("A descrição é obrigatória.");

      if (payment.Cost <= 0)
        ThrowValidationError("O valor deve ser maior que Zero.");

      if (default(DateTime) == payment.FirstPayment)
        ThrowValidationError("A data do primeiro pagamento é obrigatória.");

      if (!payment.FixedPayment)
      {
        if (payment.PlotsPaid > payment.Plots)
          ThrowValidationError("O quantidade parcelas pagas não pode ser maior que o número de parcelas.");

        if (payment.Plots <= 0)
          ThrowValidationError("O pagamento deve ter pelo menos 1 parcela.");
      }

      if (payment.CreditCardId.HasValue)
      {
        var card = _context.CreditCard.FirstOrDefault(p => p.Id == payment.CreditCardId.Value && p.UserId == UserId);
        if (card is null)
          ThrowValidationError("Cartão não localizado.");
      }
    }

    private List<string> GetMonthsFromPayment(Payment p)
    {
      List<string> months = new List<string>();
      months.Add(p.FirstPayment.ToString("MM/yyyy"));
      for (int i = 1; i < p.Plots; i++)
        months.Add(p.FirstPayment.AddMonths(i).ToString("MM/yyyy"));
      return months;
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using FinanceApi.Infra.Entity;
using FinanceApi.Infra.Repository;
using FinanceApi.Models;
using FinanceApi.Shared;

namespace Cashflow.Api.Service
{
  /// <summary>
  /// Payment service
  /// </summary>
  public class PaymentService : BaseService
  {
    private IPaymentRepository _paymentRepository;
    private ICreditCardRepository _creditCardRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    public PaymentService(IPaymentRepository paymentRepository, ICreditCardRepository creditCardRepository)
    {
      _paymentRepository = paymentRepository;
      _creditCardRepository = creditCardRepository;
    }

    /// Obter pagamentos por usuário
    public List<Payment> Get(int userId) => _paymentRepository.GetByUser(userId);

    /// Obter os pagamentos futuros agrupados pelo mês
    public Dictionary<string, PaymentFutureResultModel> GetFuturePayments(int userId, DateTime forecastAt)
    {
      var result = new Dictionary<string, PaymentFutureResultModel>();
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

      var payments = _paymentRepository.GetByUser(userId);

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
                Plots = p.Plots,
                PlotsPaid = p.PlotsPaid,
                IsCreditCard = false,
                Month = date.ToString("MM/yyyy")
              };
            }

            paymentModel.Items.Add(new PaymentItemModel()
            {
              PaymentId = p.Id,
              Description = p.Description,
              Cost = p.FixedPayment || p.SinglePlot ? p.Cost : p.Cost / p.Plots,
              Plots = p.FixedPayment || p.SinglePlot ? 0 : p.Plots,
              PlotsPaid = p.FixedPayment || p.SinglePlot ? 0 : p.PlotsPaid,
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
        result.Add(date.ToString("MM/yyyy"), resultModel);
        resultModel.AcumulatedCost = result.Values.Sum(p => p.CostIncome) - result.Values.Sum(p => p.CostExpense);
      });

      return result;
    }

    /// <summary>
    /// Inserir um novo pagamento
    /// </summary>
    public void Post(Payment payment)
    {
      ValidatePayment(payment);
      _paymentRepository.Add(payment);
      _paymentRepository.Save();
    }

    /// <summary>
    /// Atualizar um pagamento
    /// </summary>
    public void Put(Payment payment)
    {
      ValidatePayment(payment);
      var paymentDb = _paymentRepository.GetById(payment.Id);
      if (paymentDb is null || paymentDb.UserId != payment.UserId)
        ThrowValidationError("Pagamento não localizado.");
      payment.MapperTo(paymentDb);
      _paymentRepository.Update(paymentDb);
      _paymentRepository.Save();
    }

    /// <summary>
    /// Remove um cartão de crédito de um usuário
    /// </summary>
    public void Delete(int paymentId, int userId)
    {
      var payment = _paymentRepository.GetById(paymentId);
      if (payment is null || payment.UserId != userId)
        ThrowValidationError("Pagamento não localizado.");

      _paymentRepository.Remove(paymentId);
      _paymentRepository.Save();
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

      if (!payment.SinglePlot && !payment.FixedPayment)
      {
        if (payment.PlotsPaid > payment.Plots)
          ThrowValidationError("O quantidade parcelas pagas não pode ser maior que o número de parcelas.");

        if (payment.Plots <= 0)
          ThrowValidationError("O pagamento deve ter pelo menos 1 parcela.");
      }

      int cardId = payment.CreditCard?.Id ?? 0;
      if (cardId == 0)
        cardId = payment.CreditCardId.HasValue ? payment.CreditCardId.Value : 0;
      if (cardId > 0)
      {
        var card = _creditCardRepository.GetById(cardId);
        if (card is null || card.UserId != payment.UserId)
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
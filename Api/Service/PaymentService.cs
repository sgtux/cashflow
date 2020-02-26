using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Service
{
  public class PaymentService : BaseService
  {
    private IPaymentRepository _paymentRepository;
    private ICreditCardRepository _creditCardRepository;

    public PaymentService(IPaymentRepository paymentRepository, ICreditCardRepository creditCardRepository)
    {
      _paymentRepository = paymentRepository;
      _creditCardRepository = creditCardRepository;
    }

    public async Task<ResultModel> GetByUser(int userId) => new ResultDataModel<IEnumerable<Payment>>(await _paymentRepository.GetByUser(userId));

    // public Dictionary<string, PaymentFutureResultModel> GetFuturePayments(int userId, DateTime forecastAt)
    // {
    //   var result = new Dictionary<string, PaymentFutureResultModel>();
    //   var now = _paymentRepository.CurrentDate;
    //   var dates = new List<DateTime>();

    //   if (forecastAt == default(DateTime) || forecastAt < now)
    //     forecastAt = now.AddMonths(11);
    //   else
    //     forecastAt.AddMonths(1);

    //   var currentDate = now;
    //   var months = 0;
    //   while (forecastAt.Month != currentDate.Month || forecastAt.Year != currentDate.Year)
    //   {
    //     currentDate = now.AddMonths(months);
    //     dates.Add(currentDate);
    //     months++;
    //   }

    //   var payments = _paymentRepository.GetByUser(userId).Result;

    //   dates.OrderBy(p => p.Ticks).ToList().ForEach(date =>
    //   {
    //     var startDate = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
    //     var paymentsMonth = new List<PaymentFutureModel>();
    //     foreach(var p in payments)
    //     {
    //       var paymentMonths = GetMonthsFromPayment(p);
    //       if (paymentMonths.Contains(date.ToString("MM/yyyy")) || (p.FixedPayment && p.FirstPayment < startDate))
    //       {
    //         string creditCardName = p.CreditCard?.Name;
    //         PaymentFutureModel paymentModel = null;
    //         if (!string.IsNullOrEmpty(creditCardName))
    //         {
    //           paymentModel = paymentsMonth.FirstOrDefault(x => x.Description == creditCardName);
    //           if (paymentModel == null)
    //           {
    //             paymentModel = new PaymentFutureModel()
    //             {
    //               Description = creditCardName,
    //               Type = TypePayment.Expense,
    //               IsCreditCard = true,
    //               Month = date.ToString("MM/yyyy")
    //             };
    //           }
    //         }
    //         else
    //         {
    //           paymentModel = new PaymentFutureModel()
    //           {
    //             Description = p.Description,
    //             Type = p.Type,
    //             Plots = p.Plots,
    //             PlotsPaid = p.PlotsPaid,
    //             IsCreditCard = false,
    //             Month = date.ToString("MM/yyyy")
    //           };
    //         }

    //         var plotsPaid = 0;
    //         if (!p.FixedPayment && !p.SinglePlot)
    //         {
    //           var diffYears = date.Year - p.FirstPayment.Year;
    //           var diffMonths = date.Month - p.FirstPayment.Month;
    //           plotsPaid = 1 + diffMonths + (diffYears * 12);
    //         }

    //         paymentModel.PlotsPaid = plotsPaid;

    //         paymentModel.Items.Add(new PaymentItemModel()
    //         {
    //           PaymentId = p.Id,
    //           Description = p.Description,
    //           Cost = p.FixedPayment || p.SinglePlot ? p.Cost : p.Cost / p.Plots,
    //           Plots = p.FixedPayment || p.SinglePlot ? 0 : p.Plots,
    //           PlotsPaid = plotsPaid,
    //           Type = p.Type,
    //           CreditCard = creditCardName,
    //           PaymentDate = date.ToString("dd/MM/yyyy"),
    //           Month = date.ToString("MM/yyyy"),
    //           Day = p.FirstPayment.Day
    //         });
    //         if (!paymentModel.IsCreditCard
    //           || (paymentModel.IsCreditCard && !paymentsMonth.Any(x => x.Description == paymentModel.Description)))
    //           paymentsMonth.Add(paymentModel);
    //       }
    //     }
    //     var resultModel = new PaymentFutureResultModel();
    //     resultModel.Payments = paymentsMonth;
    //     result.Add(date.ToString("MM/yyyy"), resultModel);
    //     resultModel.AcumulatedCost = result.Values.Sum(p => p.CostIncome) - result.Values.Sum(p => p.CostExpense);
    //   });

    //   return result;
    // }

    /// <summary>
    /// Inserir um novo pagamento
    /// </summary>
    public async Task<ResultModel> Add(Payment payment)
    {
      var result = new ResultModel();
      var validatorResult = new PaymentValidator().Validate(payment);
      if (!validatorResult.IsValid)
        result.AddNotification(validatorResult.Errors);
      else
        await _paymentRepository.Add(payment);
      return result;
    }

    /// <summary>
    /// Atualizar um pagamento
    /// </summary>
    public async Task<ResultModel> Update(Payment payment)
    {
      var result = new ResultModel();
      var validatorResult = new PaymentValidator().Validate(payment);
      if (!validatorResult.IsValid)
        result.AddNotification(validatorResult.Errors);
      else
      {
        var paymentDb = await _paymentRepository.GetById(payment.Id);
        if (paymentDb is null || paymentDb.UserId != payment.UserId)
          result.AddNotification("Payment not found.");
        else
        {
          payment.Map(paymentDb);
          await _paymentRepository.Update(paymentDb);
        }
      }
      return result;
    }

    public async Task<ResultModel> Remove(int paymentId, int userId)
    {
      var result = new ResultModel();
      var payment = await _paymentRepository.GetById(paymentId);
      if (payment is null || payment.UserId != userId)
        result.AddNotification("Payment not found.");
      else
        await _paymentRepository.Remove(paymentId);
      return result;
    }

    // private void ValidatePayment(Payment payment)
    // {
    //   if (payment is null)
    //     ThrowValidationError("Pagamento inválido.");

    //   if (payment.Cost <= 0)
    //     ThrowValidationError("O valor deve ser maior que Zero.");

    //   if (default(DateTime) == payment.FirstPayment)
    //     ThrowValidationError("A data do primeiro pagamento é obrigatória.");

    //   if (!payment.SinglePlot && !payment.FixedPayment)
    //   {
    //     if (payment.PlotsPaid > payment.Plots)
    //       ThrowValidationError("O quantidade parcelas pagas não pode ser maior que o número de parcelas.");

    //     if (payment.Plots <= 0)
    //       ThrowValidationError("O pagamento deve ter pelo menos 1 parcela.");
    //   }

    //   int cardId = payment.CreditCard?.Id ?? 0;
    //   if (cardId == 0)
    //     cardId = payment.CreditCardId.HasValue ? payment.CreditCardId.Value : 0;
    //   if (cardId > 0)
    //   {
    //     var card = _creditCardRepository.GetById(cardId).Result;
    //     if (card is null || card.UserId != payment.UserId)
    //       ThrowValidationError("Cartão não localizado.");
    //   }
    // }

    // private List<string> GetMonthsFromPayment(Payment p)
    // {
    //   List<string> months = new List<string>();
    //   months.Add(p.FirstPayment.ToString("MM/yyyy"));
    //   for (int i = 1; i < p.Plots; i++)
    //     months.Add(p.FirstPayment.AddMonths(i).ToString("MM/yyyy"));
    //   return months;
    // }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Services
{
    public class ProjectionService : BaseService
    {
        private readonly IPaymentRepository _paymentRepository;

        private readonly ICreditCardRepository _creditCardRepository;

        private readonly IEarningRepository _earningRepository;

        private readonly IHouseholdExpenseRepository _householdExpenseRepository;

        private readonly IVehicleRepository _vehicleRepository;

        private readonly IRemainingBalanceRepository _remainingBalanceRepository;

        private readonly IRecurringExpenseRepository _recurringExpenseRepository;

        public ProjectionService(IPaymentRepository paymentRepository,
           ICreditCardRepository creditCardRepository,
           IEarningRepository earningRepository,
           IHouseholdExpenseRepository householdExpenseRepository,
           IVehicleRepository vehicleRepository,
           IRemainingBalanceRepository remainingBalanceRepository,
           IRecurringExpenseRepository recurringExpenseRepository
           )
        {
            _paymentRepository = paymentRepository;
            _creditCardRepository = creditCardRepository;
            _earningRepository = earningRepository;
            _householdExpenseRepository = householdExpenseRepository;
            _vehicleRepository = vehicleRepository;
            _remainingBalanceRepository = remainingBalanceRepository;
            _recurringExpenseRepository = recurringExpenseRepository;
        }

        public async Task<ResultDataModel<Dictionary<string, PaymentProjectionResultModel>>> GetProjection(int userId, int month, int year)
        {
            var result = new ResultDataModel<Dictionary<string, PaymentProjectionResultModel>>();
            var baseFilter = new BaseFilter() { UserId = userId };
            var types = await _paymentRepository.GetTypes();
            var cards = await _creditCardRepository.GetSome(baseFilter);

            var dates = LoadDates(month, year);
            var list = new List<PaymentProjectionModel>();

            await FillBenefits(list, dates, types, baseFilter);
            await FillPayments(list, dates, types, cards, baseFilter);
            await FillFuelExpenses(list, cards, dates, types, baseFilter);
            await FillHouseholdExpense(list, cards, dates, types, baseFilter);
            await FillRecurringExpenses(list, dates, types, cards, baseFilter);
            await FillRemainingBalance(list, dates, types, cards, baseFilter);

            dates.OrderBy(p => p.Ticks).ToList().ForEach(date =>
            {
                var monthYear = date.ToString("MM/yyyy");
                var resultModel = new PaymentProjectionResultModel();
                resultModel.Payments.AddRange(list.Where(p => p.MonthYear == monthYear));
                result.Data.Add(monthYear, resultModel);
                resultModel.AccumulatedValue = result.Data.Values.Sum(p => p.Total + p.PreviousMonthBalanceValue);
            });

            return result;
        }

        private List<DateTime> LoadDates(int month, int year)
        {
            var now = _paymentRepository.CurrentDate;
            var dates = new List<DateTime>();

            if (month <= 0 || month > 12)
                month = 12;

            if (year < now.Year || year > now.AddYears(5).Year)
                year = now.AddYears(1).Year;

            var baseDate = new DateTime(year, month, 1);

            if (baseDate == default(DateTime) || baseDate < now)
                baseDate = now.AddMonths(11);
            else
                baseDate.AddMonths(1);

            var currentDate = now;
            var months = 0;
            while (baseDate.Month != currentDate.Month || baseDate.Year != currentDate.Year)
            {
                currentDate = now.AddMonths(months);
                dates.Add(currentDate);
                months++;
            }
            return dates;
        }

        private async Task FillBenefits(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<PaymentType> types, BaseFilter filter)
        {
            var benefits = await _earningRepository.GetSome(filter);
            if (benefits.Any())
            {
                var monthyBenefits = benefits.Where(p => p.Date.SameMonthYear(DateTime.Now) && p.Type == Enums.EarningType.MonthyBenefit);
                foreach (var date in dates)
                {
                    foreach (var item in monthyBenefits)
                        list.Add(new PaymentProjectionModel()
                        {
                            Description = $"{item.Description} ({item.TypeDescription})",
                            MonthYear = date.ToString("MM/yyyy"),
                            Type = types.First(p => p.Id == (int)Enums.PaymentType.Gain),
                            Value = item.Value
                        });

                    foreach (var item in benefits.Where(p => p.Type != Enums.EarningType.MonthyBenefit && p.Date.SameMonthYear(date)))
                        list.Add(new PaymentProjectionModel()
                        {
                            Description = $"{item.Description} (Benefício)",
                            MonthYear = date.ToString("MM/yyyy"),
                            Type = types.First(p => p.Id == (int)Enums.PaymentType.Gain),
                            Value = item.Value
                        });
                }
            }
        }

        private async Task FillPayments(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<PaymentType> types, IEnumerable<CreditCard> cards, BaseFilter filter)
        {
            var payments = await _paymentRepository.GetSome(filter);
            payments = payments.Where(p => !p.Done || p.DoneInThisMonth);

            foreach (var date in dates)
            {
                Func<Installment, bool> condition = i => (i.PaidDate == null && !i.Exempt && i.Date.SameMonthYear(date)) || (i.PaidDate != null && i.PaidDate.Value.SameMonthYear(date));
                foreach (var payMonth in payments.Where(p => p.Installments?.Any(condition) ?? false))
                {
                    var installments = payMonth.Installments.Where(condition);
                    var value = (installments.Where(p => p.PaidDate.HasValue).Sum(p => p.PaidValue) ?? 0) + installments.Where(p => !p.PaidDate.HasValue).Sum(p => p.Value);

                    list.Add(new PaymentProjectionModel()
                    {
                        CreditCard = cards.FirstOrDefault(p => p.Id == payMonth.CreditCardId),
                        Description = payMonth.Description,
                        MonthYear = date.ToString("MM/yyyy"),
                        Number = string.Join("-", installments.Select(p => p.Number)),
                        PaidDate = installments.First().PaidDate,
                        QtdInstallments = payMonth.Installments.Count,
                        Type = payMonth.Type,
                        Value = value
                    });
                }
            }
        }

        private async Task FillFuelExpenses(List<PaymentProjectionModel> list, IEnumerable<CreditCard> cards, List<DateTime> dates, IEnumerable<PaymentType> types, BaseFilter filter)
        {
            var fromDate = DateTime.Now.AddMonths(-3).FixFirstDayInMonth();
            var vehicles = await _vehicleRepository.GetSome(new BaseFilter() { UserId = filter.UserId, StartDate = fromDate });
            List<FuelExpense> allFuelExpenses = new List<FuelExpense>();
            vehicles.ToList().ForEach(p => allFuelExpenses.AddRange(p.FuelExpenses));

            var average = allFuelExpenses.Where(p => p.Date.SameMonthYear(fromDate)).Sum(p => p.ValueSupplied);
            average += allFuelExpenses.Where(p => p.Date.SameMonthYear(fromDate.AddMonths(1))).Sum(p => p.ValueSupplied);
            average += allFuelExpenses.Where(p => p.Date.SameMonthYear(fromDate.AddMonths(2))).Sum(p => p.ValueSupplied);

            if (average > 0)
                average = average / 3;

            foreach (var date in dates)
            {
                var fuelExpenses = allFuelExpenses.Where(p => p.Date.SameMonthYear(date));
                if (fuelExpenses.Any())
                {
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = $"Gastos em Combustível",
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)Enums.PaymentType.Expense),
                        Value = fuelExpenses.Sum(p => p.ValueSupplied)
                    });
                }
                else if (average > 0)
                {
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = $"Gastos em Combustível (Estimado)",
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)Enums.PaymentType.Expense),
                        Value = average
                    });
                }
            }
        }

        private async Task FillHouseholdExpense(List<PaymentProjectionModel> list, IEnumerable<CreditCard> cards, List<DateTime> dates, IEnumerable<PaymentType> types, BaseFilter filter)
        {
            var fromDate = CurrentDate.AddMonths(-3).FixFirstDayInMonth();
            var allHouseholdExpenses = await _householdExpenseRepository.GetSome(new BaseFilter() { UserId = filter.UserId, StartDate = fromDate });

            var average = allHouseholdExpenses.Where(p => p.IsEstimated && p.Date.SameMonthYear(fromDate)).Sum(p => p.Value);
            average += allHouseholdExpenses.Where(p => p.IsEstimated && p.Date.SameMonthYear(fromDate.AddMonths(1))).Sum(p => p.Value);
            average += allHouseholdExpenses.Where(p => p.IsEstimated && p.Date.SameMonthYear(fromDate.AddMonths(2))).Sum(p => p.Value);

            if (average > 0)
                average = average / 3;

            var lastMonthHouseholdExpenses = allHouseholdExpenses.Where(p => p.Date.SameMonthYear(CurrentDate.AddMonths(-1)) && p.NextInvoice);
            var invoiceHouseholdExpenses = allHouseholdExpenses.Where(p => p.Date.SameMonthYear(CurrentDate));
            var nextMonthAverage = average;

            foreach (var card in cards)
            {
                var currentInvoiceSum = lastMonthHouseholdExpenses.Where(p => p.CreditCardId == card.Id).Sum(p => p.Value);
                currentInvoiceSum += invoiceHouseholdExpenses.Where(p => p.CurrentInvoice && p.CreditCardId == card.Id).Sum(p => p.Value);
                if (currentInvoiceSum > 0)
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = $"Despesas Domésticas ({card.Name})",
                        MonthYear = CurrentDate.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)Enums.PaymentType.Expense),
                        Value = currentInvoiceSum,
                        CreditCard = card
                    });

                var nextInvoiceSum = invoiceHouseholdExpenses.Where(p => p.NextInvoice && p.CreditCardId == card.Id).Sum(p => p.Value);
                if (nextInvoiceSum > 0)
                {
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = $"Despesas Domésticas ({card.Name})",
                        MonthYear = CurrentDate.AddMonths(1).ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)Enums.PaymentType.Expense),
                        Value = nextInvoiceSum,
                        CreditCard = card
                    });
                    nextMonthAverage -= nextInvoiceSum;
                }
            }

            foreach (var date in dates)
            {
                var householdExpenses = allHouseholdExpenses.Where(p => p.Date.SameMonthYear(date));

                if (householdExpenses.Any(p => !p.CreditCardId.HasValue))
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = $"Despesas Domésticas",
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)Enums.PaymentType.Expense),
                        Value = householdExpenses.Where(p => !p.CreditCardId.HasValue).Sum(p => p.Value)
                    });
                else if (average > 0 && nextMonthAverage > 0)
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = $"Despesas Domésticas (Estimado)",
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)Enums.PaymentType.Expense),
                        Value = date.SameMonthYear(CurrentDate.AddMonths(1)) ? nextMonthAverage : average
                    });
            }
        }

        private async Task FillRecurringExpenses(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<PaymentType> types, IEnumerable<CreditCard> cards, BaseFilter filter)
        {
            var currentRecurringExpenses = await _recurringExpenseRepository.GetSome(new BaseFilter() { UserId = filter.UserId, StartDate = CurrentDate.FixFirstDayInMonth(), EndDate = CurrentDate.FixLastDayInMonth() });
            var projectionRecurringExpenses = await _recurringExpenseRepository.GetSome(new BaseFilter() { UserId = filter.UserId, Active = true });
            foreach (var date in dates)
            {
                if (date.SameMonthYear(CurrentDate))
                {
                    var expenses = currentRecurringExpenses.Where(p => p.HasHistory() && p.History.Any(x => date.SameMonthYear(x.Date)));
                    foreach (var item in expenses)
                        list.Add(new PaymentProjectionModel()
                        {
                            Description = $"{item.Description} (Recorrente)",
                            CreditCard = cards.FirstOrDefault(p => p.Id == item.CreditCardId),
                            MonthYear = date.ToString("MM/yyyy"),
                            Type = types.First(p => p.Id == (int)Enums.PaymentType.Expense),
                            Value = item.History.First(p => date.SameMonthYear(p.Date)).PaidValue
                        });

                    foreach (var item in projectionRecurringExpenses.Where(p => !expenses.Select(x => x.Id).Contains(p.Id)))
                        list.Add(new PaymentProjectionModel()
                        {
                            Description = $"{item.Description} (Recorrente)",
                            CreditCard = cards.FirstOrDefault(p => p.Id == item.CreditCardId),
                            MonthYear = date.ToString("MM/yyyy"),
                            Type = types.First(p => p.Id == (int)Enums.PaymentType.Expense),
                            Value = item.Value
                        });
                }
                else
                {
                    foreach (var item in projectionRecurringExpenses)
                        list.Add(new PaymentProjectionModel()
                        {
                            Description = $"{item.Description} (Recorrente)",
                            CreditCard = cards.FirstOrDefault(p => p.Id == item.CreditCardId),
                            MonthYear = date.ToString("MM/yyyy"),
                            Type = types.First(p => p.Id == (int)Enums.PaymentType.Expense),
                            Value = item.Value
                        });
                }
            }
        }

        private async Task FillRemainingBalance(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<PaymentType> types, IEnumerable<CreditCard> cards, BaseFilter filter)
        {
            var now = CurrentDate;
            var remainingBalance = await _remainingBalanceRepository.GetByMonthYear(filter.UserId, now.AddMonths(-1));
            if (remainingBalance != null)
                list.Add(new PaymentProjectionModel()
                {
                    Description = Constants.PREVIOUS_MONTH_BALANCE,
                    MonthYear = now.ToString("MM/yyyy"),
                    Type = types.First(p => p.Id == (remainingBalance.Value >= 0 ? (int)Enums.PaymentType.Gain : (int)Enums.PaymentType.Expense)),
                    Value = Math.Abs(remainingBalance.Value)
                });
        }
    }
}
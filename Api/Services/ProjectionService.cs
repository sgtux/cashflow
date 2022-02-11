using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Enums;

namespace Cashflow.Api.Services
{
    public class ProjectionService : BaseService
    {
        private IPaymentRepository _paymentRepository;

        private ICreditCardRepository _creditCardRepository;

        private IEarningRepository _earningRepository;

        private IHouseholdExpenseRepository _householdExpenseRepository;

        private IVehicleRepository _vehicleRepository;

        private IRemainingBalanceRepository _remainingBalanceRepository;

        private IRecurringExpenseRepository _recurringExpenseRepository;

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

            await FillSalary(list, dates, types, baseFilter);
            await FillPayments(list, dates, types, cards, baseFilter);
            await FillVehicleExpenses(list, dates, types, baseFilter);
            await FillHouseholdExpense(list, dates, types, baseFilter);
            await FillRecurringExpenses(list, dates, types, cards, baseFilter);
            await FillRemainingBalance(list, dates, types, cards, baseFilter);

            dates.OrderBy(p => p.Ticks).ToList().ForEach(date =>
            {
                var monthYear = date.ToString("MM/yyyy");
                var resultModel = new PaymentProjectionResultModel();
                resultModel.Payments.AddRange(list.Where(p => p.MonthYear == monthYear));
                result.Data.Add(monthYear, resultModel);
                resultModel.AccumulatedCost = result.Data.Values.Sum(p => p.Total);
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

        private async Task FillSalary(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<PaymentType> types, BaseFilter filter)
        {
            var salary = (await _earningRepository.GetSome(filter)).FirstOrDefault(p => p.Date.SameMonthYear(DateTime.Now));
            if (salary != null)
            {
                foreach (var date in dates)
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = "Salário",
                        Monthly = true,
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)PaymentTypeEnum.Gain),
                        Cost = salary.Value
                    });
            }
        }

        private async Task FillPayments(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<PaymentType> types, IEnumerable<CreditCard> cards, BaseFilter filter)
        {
            var payments = (await _paymentRepository.GetSome(filter)).Where(p => p.Active);
            foreach (var date in dates)
                foreach (var payMonth in payments.Where(p => p.Monthly || (p.Installments?.Any(p => p.Date.Year == date.Year && p.Date.Month == date.Month) ?? false)))
                {
                    var installment = payMonth.Installments.First(p => payMonth.Monthly || (p.Date.Year == date.Year && p.Date.Month == date.Month));
                    list.Add(new PaymentProjectionModel()
                    {
                        CreditCard = cards.FirstOrDefault(p => p.Id == payMonth.CreditCardId),
                        Description = payMonth.Description,
                        Monthly = payMonth.Monthly,
                        Condition = payMonth.Condition,
                        MonthYear = date.ToString("MM/yyyy"),
                        Number = installment.Number,
                        PaidDate = installment.PaidDate,
                        QtdInstallments = payMonth.Installments.Count,
                        Type = payMonth.Type,
                        Cost = installment.Cost
                    });
                };
        }

        private async Task FillVehicleExpenses(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<PaymentType> types, BaseFilter filter)
        {
            var fromDate = DateTime.Now.AddMonths(-3).FixFirstDayInMonth();
            var vehicles = await _vehicleRepository.GetSome(new BaseFilter() { UserId = filter.UserId, StartDate = fromDate });
            List<FuelExpenses> allFuelExpenses = new List<FuelExpenses>();
            vehicles.ToList().ForEach(p => allFuelExpenses.AddRange(p.FuelExpenses));

            var average = allFuelExpenses.Where(p => p.Date.SameMonthYear(fromDate)).Sum(p => p.ValueSupplied);
            average += allFuelExpenses.Where(p => p.Date.SameMonthYear(fromDate.AddMonths(1))).Sum(p => p.ValueSupplied);
            average += allFuelExpenses.Where(p => p.Date.SameMonthYear(fromDate.AddMonths(2))).Sum(p => p.ValueSupplied);

            if (average > 0)
                average = average / 3;

            foreach (var date in dates)
            {
                var fuelExpenses = allFuelExpenses.Where(p => p.Date.Month == date.Month && p.Date.Year == date.Year);
                if (fuelExpenses.Any())
                {
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = $"Gastos em Combustível",
                        Monthly = false,
                        Condition = PaymentCondition.Cash,
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)PaymentTypeEnum.Expense),
                        Cost = fuelExpenses.Sum(p => p.ValueSupplied)
                    });
                }
                else if (average > 0)
                {
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = $"Gastos em Combustível (Estimado)",
                        Monthly = false,
                        Condition = PaymentCondition.Cash,
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)PaymentTypeEnum.Expense),
                        Cost = average
                    });
                }
            }
        }

        private async Task FillHouseholdExpense(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<PaymentType> types, BaseFilter filter)
        {
            var fromDate = CurrentDate.AddMonths(-3).FixFirstDayInMonth();
            var allHouseholdExpenses = await _householdExpenseRepository.GetSome(new BaseFilter() { UserId = filter.UserId, StartDate = fromDate });

            var average = allHouseholdExpenses.Where(p => p.Date.SameMonthYear(fromDate)).Sum(p => p.Value);
            average += allHouseholdExpenses.Where(p => p.Date.SameMonthYear(fromDate.AddMonths(1))).Sum(p => p.Value);
            average += allHouseholdExpenses.Where(p => p.Date.SameMonthYear(fromDate.AddMonths(2))).Sum(p => p.Value);

            if (average > 0)
                average = average / 3;

            foreach (var date in dates)
            {
                var householdExpense = allHouseholdExpenses.Where(p => p.Date.Month == date.Month && p.Date.Year == date.Year);
                if (householdExpense.Any())
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = $"Despesas Domésticas",
                        Monthly = false,
                        Condition = PaymentCondition.Cash,
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)PaymentTypeEnum.Expense),
                        Cost = householdExpense.Sum(p => p.Value)
                    });
                else if (average > 0)
                    list.Add(new PaymentProjectionModel()
                    {
                        Description = $"Despesas Domésticas (Estimado)",
                        Monthly = false,
                        Condition = PaymentCondition.Cash,
                        MonthYear = date.ToString("MM/yyyy"),
                        Type = types.First(p => p.Id == (int)PaymentTypeEnum.Expense),
                        Cost = average
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
                            Monthly = true,
                            CreditCard = cards.FirstOrDefault(p => p.Id == item.CreditCardId),
                            Condition = PaymentCondition.Cash,
                            MonthYear = date.ToString("MM/yyyy"),
                            Type = types.First(p => p.Id == (int)PaymentTypeEnum.Expense),
                            Cost = item.History.First(p => date.SameMonthYear(p.Date)).PaidValue
                        });

                    foreach (var item in projectionRecurringExpenses.Where(p => !expenses.Select(x => x.Id).Contains(p.Id)))
                        list.Add(new PaymentProjectionModel()
                        {
                            Description = $"{item.Description} (Recorrente)",
                            Monthly = true,
                            CreditCard = cards.FirstOrDefault(p => p.Id == item.CreditCardId),
                            Condition = PaymentCondition.Cash,
                            MonthYear = date.ToString("MM/yyyy"),
                            Type = types.First(p => p.Id == (int)PaymentTypeEnum.Expense),
                            Cost = item.Value
                        });
                }
                else
                {
                    foreach (var item in projectionRecurringExpenses)
                        list.Add(new PaymentProjectionModel()
                        {
                            Description = $"{item.Description} (Recorrente)",
                            Monthly = true,
                            CreditCard = cards.FirstOrDefault(p => p.Id == item.CreditCardId),
                            Condition = PaymentCondition.Cash,
                            MonthYear = date.ToString("MM/yyyy"),
                            Type = types.First(p => p.Id == (int)PaymentTypeEnum.Expense),
                            Cost = item.Value
                        });
                }
            }
        }

        private async Task FillRemainingBalance(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<PaymentType> types, IEnumerable<CreditCard> cards, BaseFilter filter)
        {
            var now = CurrentDate;
            var remainingBalance = await _remainingBalanceRepository.GetByMonthYear(filter.UserId, now.Month, now.Year);
            if (remainingBalance != null)
                list.Add(new PaymentProjectionModel()
                {
                    Description = "Saldo Mês Anterior",
                    MonthYear = now.ToString("MM/yyyy"),
                    Type = types.First(p => p.Id == (remainingBalance.Value >= 0 ? (int)PaymentTypeEnum.Gain : (int)PaymentTypeEnum.Expense)),
                    Cost = Math.Abs(remainingBalance.Value)
                });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Enums;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;
using Cashflow.Api.Shared.Cache;
using Cashflow.Api.Utils;

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

        private readonly IUserRepository _userRepository;

        private readonly AppCache _appCache;

        public ProjectionService(IPaymentRepository paymentRepository,
           ICreditCardRepository creditCardRepository,
           IEarningRepository earningRepository,
           IHouseholdExpenseRepository householdExpenseRepository,
           IVehicleRepository vehicleRepository,
           IRemainingBalanceRepository remainingBalanceRepository,
           IRecurringExpenseRepository recurringExpenseRepository,
           IUserRepository userRepository,
           AppCache appCache
           )
        {
            _paymentRepository = paymentRepository;
            _creditCardRepository = creditCardRepository;
            _earningRepository = earningRepository;
            _householdExpenseRepository = householdExpenseRepository;
            _vehicleRepository = vehicleRepository;
            _remainingBalanceRepository = remainingBalanceRepository;
            _recurringExpenseRepository = recurringExpenseRepository;
            _userRepository = userRepository;
            _appCache = appCache;
        }

        public async Task<ResultDataModel<List<PaymentMonthProjectionModel>>> GetProjection(int userId)
        {
            var monthPaymentList = _appCache.Projection.Get(userId);
            if (monthPaymentList != null)
                return new ResultDataModel<List<PaymentMonthProjectionModel>>(monthPaymentList, true);

            monthPaymentList = new List<PaymentMonthProjectionModel>();
            var baseFilter = new BaseFilter() { UserId = userId };
            var cards = await _creditCardRepository.GetSome(baseFilter);
            var user = await _userRepository.GetById(userId);

            var dates = LoadDates(12, DateTimeUtils.CurrentDate.Year + 1);
            var list = new List<PaymentProjectionModel>();

            await FillEarnings(list, dates, baseFilter);
            await FillPayments(list, dates, cards, baseFilter);
            await FillFuelExpenses(list, dates, baseFilter, user);
            await FillHouseholdExpense(list, dates, cards, baseFilter, user);
            await FillRecurringExpenses(list, dates, cards, baseFilter);
            await FillRemainingBalance(list, baseFilter);

            var paymentMonthList = new List<PaymentMonthProjectionModel>();

            dates.OrderBy(p => p.Ticks).ToList().ForEach(date =>
            {
                var monthYear = date.ToString("MM/yyyy");
                var monthPayment = new PaymentMonthProjectionModel(monthYear, list);
                monthPaymentList.Add(monthPayment);
                monthPayment.AccumulatedValue = monthPaymentList.Sum(p => p.Total + p.PreviousMonthBalanceValue);
            });

            _appCache.Projection.Update(userId, monthPaymentList);

            return new ResultDataModel<List<PaymentMonthProjectionModel>>(monthPaymentList);
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

            if (baseDate == default || baseDate < now)
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

        private async Task FillEarnings(List<PaymentProjectionModel> list, List<DateTime> dates, BaseFilter filter)
        {
            var earnings = await _earningRepository.GetSome(filter);
            if (earnings.Any())
            {
                List<Earning> monthyEarnings = new List<Earning>();
                foreach (var date in dates.Order())
                {
                    monthyEarnings.AddRange(earnings.Where(p => p.Date.SameMonthYear(date) && p.Type == EarningType.Monthy));
                    foreach (var item in monthyEarnings)
                        list.Add(new PaymentProjectionModel($"{item.Description} ({item.TypeDescription})", date, item.Value, MovementProjectionType.Earning));

                    foreach (var item in earnings.Where(p => p.Type != EarningType.Monthy && p.Date.SameMonthYear(date)))
                        list.Add(new PaymentProjectionModel($"{item.Description} (Provento)", date, item.Value, MovementProjectionType.Earning));
                }
            }
        }

        private async Task FillPayments(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<CreditCard> cards, BaseFilter filter)
        {
            var payments = await _paymentRepository.GetSome(new PaymentFilter(filter));
            payments = payments.Where(p => !p.Done || p.DoneInThisMonth);

            foreach (var date in dates)
            {
                Func<Installment, bool> condition = i => (i.PaidDate == null && !i.Exempt && i.Date.SameMonthYear(date)) || (i.PaidDate != null && i.PaidDate.Value.SameMonthYear(date));
                foreach (var payMonth in payments.Where(p => p.Installments?.Any(condition) ?? false))
                {
                    var installments = payMonth.Installments.Where(condition);
                    var value = (installments.Where(p => p.PaidDate.HasValue).Sum(p => p.PaidValue) ?? 0) + installments.Where(p => !p.PaidDate.HasValue).Sum(p => p.Value);
                    var creditCard = cards.FirstOrDefault(p => p.Id == payMonth.CreditCardId);
                    var number = string.Join("-", installments.Select(p => p.Number));
                    var paidDate = installments.First().PaidDate;
                    var qtdInstallments = payMonth.Installments.Count;
                    var qtdPaidInstallments = payMonth.PaidInstallments;

                    list.Add(new PaymentProjectionModel(payMonth.Description, date, value, MovementProjectionType.Payment, creditCard, number, paidDate, qtdInstallments, qtdPaidInstallments));
                }
            }
        }

        private async Task FillFuelExpenses(List<PaymentProjectionModel> list, List<DateTime> dates, BaseFilter filter, User user)
        {
            var fromDate = DateTimeUtils.CurrentDate.AddMonths(-3).FixFirstDayInMonth();
            var vehicles = await _vehicleRepository.GetSome(new BaseFilter() { UserId = filter.UserId, StartDate = fromDate });
            List<FuelExpense> allFuelExpenses = new List<FuelExpense>();
            vehicles.ToList().ForEach(p => allFuelExpenses.AddRange(p.FuelExpenses));

            foreach (var date in dates)
            {
                var fuelExpenses = allFuelExpenses.Where(p => p.Date.SameMonthYear(date));
                if (fuelExpenses.Any())
                    list.Add(new PaymentProjectionModel("Gastos em Combustível", date, fuelExpenses.Sum(p => p.ValueSupplied), MovementProjectionType.FuelExpense));
                else
                    list.Add(new PaymentProjectionModel("Gastos em Combustível (Residual)", date, user.FuelExpenseLimit, MovementProjectionType.FuelExpense));
            }
        }

        private async Task FillHouseholdExpense(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<CreditCard> cards, BaseFilter filter, User user)
        {
            var fromDate = CurrentDate.AddMonths(-3).FixFirstDayInMonth();
            var allHouseholdExpenses = await _householdExpenseRepository.GetSome(new HouseholdExpenseFilter() { UserId = filter.UserId, StartDate = fromDate });

            foreach (var date in dates)
            {
                var householdExpenses = allHouseholdExpenses.Where(p => p.Date.SameMonthYear(date) || p.InvoiceDate.SameMonthYear(date));
                decimal totalInCard = 0;
                decimal valueWithoutCard = 0;

                if (householdExpenses.Any())
                {
                    foreach (var item in householdExpenses.Where(p => p.CreditCardId.HasValue))
                    {
                        if (!list.Any(p => p.Id == item.Id) && item.InvoiceDate.SameMonthYear(date))
                        {
                            list.Add(new PaymentProjectionModel($"{item.Description} (Despesas Domésticas)", item.InvoiceDate, item.Value, MovementProjectionType.HouseholdExpense, item.CreditCard, item.Id));
                            totalInCard += item.Value;
                        }
                    }
                    valueWithoutCard = householdExpenses.Where(p => !p.CreditCardId.HasValue).Sum(p => p.Value);
                    if (valueWithoutCard > 0)
                        list.Add(new PaymentProjectionModel("Despesas Domésticas", date, valueWithoutCard, MovementProjectionType.HouseholdExpense));
                }
                decimal residual = user.ExpenseLimit - valueWithoutCard - totalInCard;
                if (residual > 0 && residual == user.ExpenseLimit)
                    list.Add(new PaymentProjectionModel("Despesas Domésticas (Residual)", date, residual, MovementProjectionType.HouseholdExpense));
            }
        }

        private async Task FillRecurringExpenses(List<PaymentProjectionModel> list, List<DateTime> dates, IEnumerable<CreditCard> cards, BaseFilter filter)
        {
            var currentRecurringExpenses = await _recurringExpenseRepository.GetSome(new RecurringExpenseFilter() { UserId = filter.UserId, StartDate = CurrentDate.FixFirstDayInMonth(), EndDate = CurrentDate.FixLastDayInMonth() });
            var projectionRecurringExpenses = await _recurringExpenseRepository.GetSome(new RecurringExpenseFilter() { UserId = filter.UserId, Active = 1 });
            foreach (var date in dates)
            {
                if (date.SameMonthYear(CurrentDate))
                {
                    var expenses = currentRecurringExpenses.Where(p => p.HasHistory() && p.History.Any(x => date.SameMonthYear(x.Date)));
                    foreach (var item in expenses)
                    {
                        var creditCard = cards.FirstOrDefault(p => p.Id == item.CreditCardId);
                        var value = item.History.First(p => date.SameMonthYear(p.Date)).PaidValue;
                        list.Add(new PaymentProjectionModel($"{item.Description} (Recorrente)", date, value, MovementProjectionType.RecurringExpenses, creditCard));
                    }

                    foreach (var item in projectionRecurringExpenses.Where(p => !expenses.Select(x => x.Id).Contains(p.Id)))
                    {
                        var creditCard = cards.FirstOrDefault(p => p.Id == item.CreditCardId);
                        list.Add(new PaymentProjectionModel($"{item.Description} (Recorrente)", date, item.Value, MovementProjectionType.RecurringExpenses, creditCard));
                    }
                }
                else
                {
                    foreach (var item in projectionRecurringExpenses)
                    {
                        var creditCard = cards.FirstOrDefault(p => p.Id == item.CreditCardId);
                        list.Add(new PaymentProjectionModel($"{item.Description} (Recorrente)", date, item.Value, MovementProjectionType.RecurringExpenses, creditCard));
                    }
                }
            }
        }

        private async Task FillRemainingBalance(List<PaymentProjectionModel> list, BaseFilter filter)
        {
            var now = CurrentDate;
            var remainingBalance = await _remainingBalanceRepository.GetByMonthYear(filter.UserId, now.AddMonths(-1));
            var type = remainingBalance.Value >= 0 ? MovementProjectionType.RemainingBalanceIn : MovementProjectionType.RemainingBalanceOut;
            if (remainingBalance != null)
                list.Add(new PaymentProjectionModel(Constants.PREVIOUS_MONTH_BALANCE, now, Math.Abs(remainingBalance.Value), type));
        }
    }
}
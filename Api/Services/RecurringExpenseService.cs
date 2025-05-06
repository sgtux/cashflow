using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Shared.Cache;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class RecurringExpenseService
    {
        private readonly IRecurringExpenseRepository _recurringExpenseRepository;

        private readonly ICreditCardRepository _creditCardRepository;

        private readonly AppCache _appCache;

        public RecurringExpenseService(
            IRecurringExpenseRepository recurringExpenseRepository,
            ICreditCardRepository creditCardRepository,
            AppCache appCache)
        {
            _recurringExpenseRepository = recurringExpenseRepository;
            _creditCardRepository = creditCardRepository;
            _appCache = appCache;
        }

        public async Task<ResultDataModel<RecurringExpense>> GetById(long id, int userId)
        {
            var expense = await _recurringExpenseRepository.GetById(id);
            return new ResultDataModel<RecurringExpense>(expense?.UserId == userId ? expense : null);
        }

        public async Task<ResultDataModel<IEnumerable<RecurringExpense>>> GetByUser(int userId, byte? active, IEnumerable<int> creditCardIds = null) => new ResultDataModel<IEnumerable<RecurringExpense>>(await _recurringExpenseRepository.GetSome(new RecurringExpenseFilter() { UserId = userId, Active = active, CreditCardIds = creditCardIds }));

        public async Task<ResultModel> Add(RecurringExpense recurringExpense)
        {
            var result = new ResultModel();
            var validatorResult = new RecurringExpenseValidator(_recurringExpenseRepository, _creditCardRepository).Validate(recurringExpense);

            if (validatorResult.IsValid)
            {
                await _recurringExpenseRepository.Add(recurringExpense);
                _appCache.Clear(recurringExpense.UserId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Update(RecurringExpense recurringExpense)
        {
            var result = new ResultModel();
            var validatorResult = new RecurringExpenseValidator(_recurringExpenseRepository, _creditCardRepository).Validate(recurringExpense);

            if (validatorResult.IsValid)
            {
                await _recurringExpenseRepository.Update(recurringExpense);
                _appCache.Clear(recurringExpense.UserId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Remove(int id, int userId)
        {
            var result = new ResultModel();

            var recurringExpense = await _recurringExpenseRepository.GetById(id);
            if (recurringExpense is null || recurringExpense.UserId != userId)
                result.AddNotification(ValidatorMessages.NotFound("Despesa Recorrente"));
            else if (recurringExpense.History != null && recurringExpense.History.Any())
                result.AddNotification(ValidatorMessages.RecurringExpense.HasHistory);
            else
            {
                await _recurringExpenseRepository.Remove(id);
                _appCache.Clear(recurringExpense.UserId);
            }

            return result;
        }

        public async Task<ResultModel> AddHistory(RecurringExpenseHistory history, int userId)
        {
            var result = new ResultModel();
            var validatorResult = new RecurringExpenseHistoryValidator(_recurringExpenseRepository, userId).Validate(history);

            if (validatorResult.IsValid)
            {
                await _recurringExpenseRepository.AddHistory(history);
                _appCache.Clear(userId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> UpdateHistory(RecurringExpenseHistory history, int userId)
        {
            var result = new ResultModel();
            var validatorResult = new RecurringExpenseHistoryValidator(_recurringExpenseRepository, userId).Validate(history);

            if (validatorResult.IsValid)
            {
                await _recurringExpenseRepository.UpdateHistory(history);
                _appCache.Clear(userId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> RemoveHistory(int id, int recurringExpenseId, int userId)
        {
            var result = new ResultModel();

            var recurringExpense = await _recurringExpenseRepository.GetById(recurringExpenseId);
            if (recurringExpense == null || recurringExpense.UserId != userId)
            {
                result.AddNotification(ValidatorMessages.NotFound("Despesa Recorrente"));
                return result;
            }

            if (recurringExpense.History.Any(p => p.Id == id))
            {
                await _recurringExpenseRepository.RemoveHistory(id);
                _appCache.Clear(userId);
            }
            else
                result.AddNotification(ValidatorMessages.NotFound("Histórico"));

            return result;
        }
    }
}
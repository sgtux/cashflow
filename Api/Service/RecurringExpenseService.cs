using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Service
{
    public class RecurringExpenseService
    {
        private IRecurringExpenseRepository _recurringExpenseRepository;

        private ICreditCardRepository _creditCardRepository;

        public RecurringExpenseService(IRecurringExpenseRepository recurringExpenseRepository, ICreditCardRepository creditCardRepository)
        {
            _recurringExpenseRepository = recurringExpenseRepository;
            _creditCardRepository = creditCardRepository;
        }

        public async Task<ResultDataModel<RecurringExpense>> GetById(long id, int userId)
        {
            var expense = await _recurringExpenseRepository.GetById(id);
            return new ResultDataModel<RecurringExpense>(expense?.UserId == userId ? expense : null);
        }

        public async Task<ResultDataModel<IEnumerable<RecurringExpense>>> GetByUser(int userId) => new ResultDataModel<IEnumerable<RecurringExpense>>(await _recurringExpenseRepository.GetSome(new BaseFilter() { UserId = userId }));

        public async Task<ResultModel> Add(RecurringExpense recurringExpense)
        {
            var result = new ResultModel();
            var validatorResult = new RecurringExpenseValidator(_recurringExpenseRepository, _creditCardRepository).Validate(recurringExpense);

            if (validatorResult.IsValid)
                await _recurringExpenseRepository.Add(recurringExpense);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Update(RecurringExpense recurringExpense)
        {
            var result = new ResultModel();
            var validatorResult = new RecurringExpenseValidator(_recurringExpenseRepository, _creditCardRepository).Validate(recurringExpense);

            if (validatorResult.IsValid)
                await _recurringExpenseRepository.Update(recurringExpense);
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
                await _recurringExpenseRepository.Remove(id);

            return result;
        }

        public async Task<ResultModel> AddHistory(RecurringExpenseHistory history, int userId)
        {
            var result = new ResultModel();
            var validatorResult = new RecurringExpenseHistoryValidator(_recurringExpenseRepository, userId).Validate(history);

            if (validatorResult.IsValid)
                await _recurringExpenseRepository.AddHistory(history);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> UpdateHistory(RecurringExpenseHistory history, int userId)
        {
            var result = new ResultModel();
            var validatorResult = new RecurringExpenseHistoryValidator(_recurringExpenseRepository, userId).Validate(history);

            if (validatorResult.IsValid)
                await _recurringExpenseRepository.UpdateHistory(history);
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
                await _recurringExpenseRepository.RemoveHistory(id);
            else
                result.AddNotification(ValidatorMessages.NotFound("Histórico"));

            return result;
        }
    }
}
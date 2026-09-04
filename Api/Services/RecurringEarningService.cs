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
    public class RecurringEarningService
    {
        private readonly IRecurringEarningRepository _recurringEarningRepository;

        private readonly AppCache _appCache;

        public RecurringEarningService(IRecurringEarningRepository recurringEarningRepository, AppCache appCache)
        {
            _recurringEarningRepository = recurringEarningRepository;
            _appCache = appCache;
        }

        public async Task<ResultDataModel<RecurringEarningEntity>> GetById(long id, int userId)
        {
            var earning = await _recurringEarningRepository.GetById(id);
            return new ResultDataModel<RecurringEarningEntity>(earning?.UserId == userId ? earning : null);
        }

        public async Task<ResultDataModel<IEnumerable<RecurringEarningEntity>>> GetByUser(int userId, byte? active) => new ResultDataModel<IEnumerable<RecurringEarningEntity>>(await _recurringEarningRepository.GetSome(new RecurringEarningFilter() { UserId = userId, Active = active }));

        public async Task<ResultModel> Add(RecurringEarningEntity recurringEarning)
        {
            var result = new ResultModel();
            var validatorResult = await new RecurringEarningValidator(_recurringEarningRepository).ValidateAsync(recurringEarning);

            if (validatorResult.IsValid)
            {
                await _recurringEarningRepository.Add(recurringEarning);
                _appCache.Clear(recurringEarning.UserId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Update(RecurringEarningEntity recurringEarning)
        {
            var result = new ResultModel();
            var validatorResult = await new RecurringEarningValidator(_recurringEarningRepository).ValidateAsync(recurringEarning);

            if (validatorResult.IsValid)
            {
                await _recurringEarningRepository.Update(recurringEarning);
                _appCache.Clear(recurringEarning.UserId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Remove(int id, int userId)
        {
            var result = new ResultModel();

            var recurringEarning = await _recurringEarningRepository.GetById(id);
            if (recurringEarning is null || recurringEarning.UserId != userId)
                result.AddNotification(ValidatorMessages.NotFound("Provento Recorrente"));
            else
            {
                await _recurringEarningRepository.Remove(id);
                _appCache.Clear(recurringEarning.UserId);
            }

            return result;
        }

        public async Task<ResultModel> AddHistory(RecurringEarningHistoryEntity history, int userId)
        {
            var result = new ResultModel();
            var validatorResult = await new RecurringEarningHistoryValidator(_recurringEarningRepository, userId).ValidateAsync(history);

            if (validatorResult.IsValid)
            {
                await _recurringEarningRepository.AddHistory(history);
                _appCache.Clear(userId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> UpdateHistory(RecurringEarningHistoryEntity history, int userId)
        {
            var result = new ResultModel();
            var validatorResult = await new RecurringEarningHistoryValidator(_recurringEarningRepository, userId).ValidateAsync(history);

            if (validatorResult.IsValid)
            {
                await _recurringEarningRepository.UpdateHistory(history);
                _appCache.Clear(userId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> RemoveHistory(int id, int recurringEarningId, int userId)
        {
            var result = new ResultModel();

            var recurringEarning = await _recurringEarningRepository.GetById(recurringEarningId);
            if (recurringEarning == null || recurringEarning.UserId != userId)
            {
                result.AddNotification(ValidatorMessages.NotFound("Provento Recorrente"));
                return result;
            }

            if (recurringEarning.History.Any(p => p.Id == id))
            {
                await _recurringEarningRepository.RemoveHistory(id);
                _appCache.Clear(userId);
            }
            else
                result.AddNotification(ValidatorMessages.NotFound("Histórico"));

            return result;
        }
    }
}

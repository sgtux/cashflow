using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Shared.Cache;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class EarningService : BaseService
    {
        private readonly IEarningRepository _earningRepository;

        private readonly AppCache _appCache;

        public EarningService(IEarningRepository earningRepository, AppCache appCache)
        {
            _earningRepository = earningRepository;
            _appCache = appCache;
        }

        public async Task<ResultDataModel<EarningEntity>> GetById(int id, int userId)
        {
            var earning = await _earningRepository.GetById(id);
            return new ResultDataModel<EarningEntity>(earning?.UserId == userId ? earning : null);
        }

        public async Task<ResultDataModel<IEnumerable<EarningEntity>>> GetByUser(int userId, DateTime? from) => new ResultDataModel<IEnumerable<EarningEntity>>(await _earningRepository.GetSome(new BaseFilter() { StartDate = from.FixStartTimeFilter(), UserId = userId }));

        public async Task<ResultModel> Add(EarningEntity earning)
        {
            var result = new ResultModel();
            var validatorResult = await new EarningValidator(_earningRepository).ValidateAsync(earning);
            if (validatorResult.IsValid)
            {
                await _earningRepository.Add(earning);
                _appCache.Clear(earning.UserId);
            }
            else
                result.AddNotification(validatorResult.Errors);
            return result;
        }

        public async Task<ResultModel> Update(EarningEntity earning)
        {
            var result = new ResultModel();
            var validatorResult = await new EarningValidator(_earningRepository).ValidateAsync(earning);
            if (validatorResult.IsValid)
            {
                await _earningRepository.Update(earning);
                _appCache.Clear(earning.UserId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Remove(int earningId, int userId)
        {
            var result = new ResultModel();

            var earning = await _earningRepository.GetById(earningId);
            if (earning is null || earning.UserId != userId)
                result.AddNotification(ValidatorMessages.NotFound("Provento"));
            else
            {
                await _earningRepository.Remove(earningId);
                _appCache.Clear(earning.UserId);
            }
            return result;
        }
    }
}
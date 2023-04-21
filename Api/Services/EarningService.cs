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

        public async Task<ResultDataModel<Earning>> GetById(int id) => new ResultDataModel<Earning>(await _earningRepository.GetById(id));

        public async Task<ResultDataModel<IEnumerable<Earning>>> GetByUser(int userId, DateTime? from) => new ResultDataModel<IEnumerable<Earning>>(await _earningRepository.GetSome(new BaseFilter() { StartDate = from.FixStartTimeFilter(), UserId = userId }));

        public ResultDataModel<IEnumerable<TypeModel>> GetTypes()
        {
            var types = Enum.GetValues<EarningType>().Select(p => new TypeModel(p));
            return new ResultDataModel<IEnumerable<TypeModel>>(types);
        }

        public async Task<ResultModel> Add(Earning earning)
        {
            var result = new ResultModel();
            var validatorResult = new EarningValidator(_earningRepository).Validate(earning);
            if (validatorResult.IsValid)
            {
                await _earningRepository.Add(earning);
                _appCache.Clear(earning.UserId);
            }
            else
                result.AddNotification(validatorResult.Errors);
            return result;
        }

        public async Task<ResultModel> Update(Earning earning)
        {
            var result = new ResultModel();
            var validatorResult = new EarningValidator(_earningRepository).Validate(earning);
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
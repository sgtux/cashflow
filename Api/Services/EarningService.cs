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
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class EarningService : BaseService
    {
        private readonly IEarningRepository _earningRepository;

        public EarningService(IEarningRepository earningRepository) => _earningRepository = earningRepository;

        public async Task<ResultDataModel<Earning>> GetById(int id) => new ResultDataModel<Earning>(await _earningRepository.GetById(id));

        public async Task<ResultDataModel<IEnumerable<Earning>>> GetByUser(int userId, DateTime? from) => new ResultDataModel<IEnumerable<Earning>>(await _earningRepository.GetSome(new BaseFilter() { StartDate = from.FixStartTimeFilter(), UserId = userId }));

        public ResultDataModel<IEnumerable<TypeModel>> GetTypes()
        {
            var types = Enum.GetValues<EarningType>().Select(p => new TypeModel()
            {
                Id = (int)p,
                Description = p.GetDescription()
            });
            return new ResultDataModel<IEnumerable<TypeModel>>(types);
        }

        public async Task<ResultModel> Add(Earning earning)
        {
            var result = new ResultModel();
            var validatorResult = new EarningValidator(_earningRepository).Validate(earning);
            if (validatorResult.IsValid)
                await _earningRepository.Add(earning);
            else
                result.AddNotification(validatorResult.Errors);
            return result;
        }

        public async Task<ResultModel> Update(Earning earning)
        {
            var result = new ResultModel();
            var validatorResult = new EarningValidator(_earningRepository).Validate(earning);
            if (validatorResult.IsValid)
                await _earningRepository.Update(earning);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Remove(int earningId, int userId)
        {
            var result = new ResultModel();

            var earning = await _earningRepository.GetById(earningId);
            if (earning is null || earning.UserId != userId)
                result.AddNotification(ValidatorMessages.NotFound("Benefício/Salário"));
            else
                await _earningRepository.Remove(earningId);
            return result;
        }
    }
}
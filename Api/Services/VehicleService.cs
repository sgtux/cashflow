using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Extensions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Utils;
using Cashflow.Api.Shared.Cache;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class VehicleService : BaseService
    {
        private readonly IVehicleRepository _vehicleRepository;
        
        private readonly AppCache _appCache;

        public VehicleService(
            IVehicleRepository vehicleRepository,
            AppCache appCache)
        {
            _vehicleRepository = vehicleRepository;
            _appCache = appCache;
        }

        public async Task<ResultModel> Add(VehicleEntity vehicle)
        {
            var result = new ResultModel();
            var validatorResult = await new VehicleValidator(_vehicleRepository).ValidateAsync(vehicle);

            if (validatorResult.IsValid)
            {
                await _vehicleRepository.Add(vehicle);
                _appCache.Clear(vehicle.UserId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultDataModel<VehicleEntity>> GetById(int id, int userId)
        {
            var p = await _vehicleRepository.GetById(id);
            return new ResultDataModel<VehicleEntity>(p?.UserId == userId ? p : null);
        }

        public async Task<ResultDataModel<IEnumerable<VehicleEntity>>> GetByUserId(int userId, bool showInactives)
        {
            var filter = new BaseFilter()
            {
                UserId = userId,
                StartDate = DateTimeUtils.CurrentDate.AddMonths(-2).FixFirstDayInMonth(),
                Active = showInactives ? null : 1
            };
            return new ResultDataModel<IEnumerable<VehicleEntity>>(await _vehicleRepository.GetSome(filter));
        }

        public async Task<ResultModel> Update(VehicleEntity vehicle)
        {
            var result = new ResultModel();
            var validatorResult = await new VehicleValidator(_vehicleRepository).ValidateAsync(vehicle);

            if (validatorResult.IsValid)
            {
                await _vehicleRepository.Update(vehicle);
                _appCache.Clear(vehicle.UserId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Delete(int id, int userId)
        {
            var result = new ResultModel();

            var vehicle = await _vehicleRepository.GetById(id);
            if (vehicle is null || vehicle.UserId != userId)
                result.AddNotification(ValidatorMessages.NotFound("Veículo"));
            else if (vehicle.FuelExpenses.Any())
                result.AddNotification(ValidatorMessages.Vehicle.HasFuelExpenses);
            else
            {
                await _vehicleRepository.Remove(id);
                _appCache.Clear(vehicle.UserId);
            }

            return result;
        }
    }
}
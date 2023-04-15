using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Shared;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class FuelExpenseService : BaseService
    {
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IFuelExpenseRepository _fuelExpenseRepository;

        private readonly ICreditCardRepository _creditCardRepository;

        private readonly ProjectionCache _projectionCache;

        public FuelExpenseService(IVehicleRepository vehicleRepository,
            IFuelExpenseRepository fuelExpenseRepository,
            ICreditCardRepository creditCardRepository,
            ProjectionCache projectionCache)
        {
            _vehicleRepository = vehicleRepository;
            _fuelExpenseRepository = fuelExpenseRepository;
            _creditCardRepository = creditCardRepository;
            _projectionCache = projectionCache;
        }

        public async Task<ResultModel> Add(FuelExpense fuelExpense, int userId)
        {
            var result = new ResultModel();
            var validatorResult = new FuelExpenseValidator(_vehicleRepository, _fuelExpenseRepository, _creditCardRepository, userId).Validate(fuelExpense);

            if (validatorResult.IsValid)
            {
                await _fuelExpenseRepository.Add(fuelExpense);
                _projectionCache.Clear(userId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Update(FuelExpense fuelExpense, int userId)
        {
            var result = new ResultModel();
            var validatorResult = new FuelExpenseValidator(_vehicleRepository, _fuelExpenseRepository, _creditCardRepository, userId).Validate(fuelExpense);

            if (validatorResult.IsValid)
            {
                await _fuelExpenseRepository.Update(fuelExpense);
                _projectionCache.Clear(userId);
            }
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Remove(int id, int userId)
        {
            var result = new ResultModel();

            var fuelExpense = await _fuelExpenseRepository.GetById(id);
            if (fuelExpense is null)
            {
                result.AddNotification(ValidatorMessages.NotFound("Despesa de Combustível"));
                return result;
            }

            var vehicle = await _vehicleRepository.GetById(fuelExpense.VehicleId);
            if (vehicle is null || vehicle.UserId != userId)
                result.AddNotification(ValidatorMessages.NotFound("Despesa de Combustível"));
            else
            {
                await _fuelExpenseRepository.Remove(id);
                _projectionCache.Clear(userId);
            }
            return result;
        }
    }
}
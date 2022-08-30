using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class FuelExpensesService : BaseService
    {
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IFuelExpensesRepository _fuelExpensesRepository;

        private readonly ICreditCardRepository _creditCardRepository;

        public FuelExpensesService(IVehicleRepository vehicleRepository,
            IFuelExpensesRepository fuelExpensesRepository,
            ICreditCardRepository creditCardRepository)
        {
            _vehicleRepository = vehicleRepository;
            _fuelExpensesRepository = fuelExpensesRepository;
            _creditCardRepository = creditCardRepository;
        }

        public async Task<ResultModel> Add(FuelExpenses fuelExpenses, int userId)
        {
            var result = new ResultModel();
            var validatorResult = new FuelExpensesValidator(_vehicleRepository, _fuelExpensesRepository, _creditCardRepository, userId).Validate(fuelExpenses);

            if (validatorResult.IsValid)
                await _fuelExpensesRepository.Add(fuelExpenses);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Update(FuelExpenses fuelExpenses, int userId)
        {
            var result = new ResultModel();
            var validatorResult = new FuelExpensesValidator(_vehicleRepository, _fuelExpensesRepository, _creditCardRepository, userId).Validate(fuelExpenses);

            if (validatorResult.IsValid)
                await _fuelExpensesRepository.Update(fuelExpenses);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultModel> Remove(int id, int userId)
        {
            var result = new ResultModel();

            var fuelExpenses = await _fuelExpensesRepository.GetById(id);
            if (fuelExpenses is null)
            {
                result.AddNotification(ValidatorMessages.NotFound("Despesa de Combustível"));
                return result;
            }

            var vehicle = await _vehicleRepository.GetById(fuelExpenses.VehicleId);
            if (vehicle is null || vehicle.UserId != userId)
                result.AddNotification(ValidatorMessages.NotFound("Despesa de Combustível"));
            else
                await _fuelExpensesRepository.Remove(id);
            return result;
        }
    }
}
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Service
{
    public class FuelExpensesService : BaseService
    {
        private IVehicleRepository _vehicleRepository;

        private IFuelExpensesRepository _fuelExpensesRepository;

        public FuelExpensesService(IVehicleRepository vehicleRepository, IFuelExpensesRepository fuelExpensesRepository)
        {
            _vehicleRepository = vehicleRepository;
            _fuelExpensesRepository = fuelExpensesRepository;
        }

        public async Task<ResultModel> Add(FuelExpenses fuelExpenses, int userId)
        {
            var result = new ResultModel();
            var validatorResult = new FuelExpensesValidator(_vehicleRepository, _fuelExpensesRepository, userId).Validate(fuelExpenses);

            if (validatorResult.IsValid)
                await _fuelExpensesRepository.Add(fuelExpenses);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }
    }
}
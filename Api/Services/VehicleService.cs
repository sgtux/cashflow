using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Services
{
    public class VehicleService : BaseService
    {
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IUserRepository _userRepository;

        public VehicleService(IVehicleRepository vehicleRepository, IUserRepository userRepository)
        {
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
        }

        public async Task<ResultModel> Add(Vehicle vehicle)
        {
            var result = new ResultModel();
            var validatorResult = new VehicleValidator(_vehicleRepository, _userRepository).Validate(vehicle);

            if (validatorResult.IsValid)
                await _vehicleRepository.Add(vehicle);
            else
                result.AddNotification(validatorResult.Errors);

            return result;
        }

        public async Task<ResultDataModel<Vehicle>> GetById(int id, int userId)
        {
            var p = await _vehicleRepository.GetById(id);
            return new ResultDataModel<Vehicle>(p?.UserId == userId ? p : null);
        }

        public async Task<ResultDataModel<IEnumerable<Vehicle>>> GetByUserId(int userId) => new ResultDataModel<IEnumerable<Vehicle>>(await _vehicleRepository.GetSome(new BaseFilter() { UserId = userId }));

        public async Task<ResultModel> Update(Vehicle vehicle)
        {
            var result = new ResultModel();
            var validatorResult = new VehicleValidator(_vehicleRepository, _userRepository).Validate(vehicle);

            if (validatorResult.IsValid)
                await _vehicleRepository.Update(vehicle);
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
                await _vehicleRepository.Remove(id);

            return result;
        }
    }
}
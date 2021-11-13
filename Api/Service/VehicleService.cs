using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using Cashflow.Api.Models;
using Cashflow.Api.Validators;

namespace Cashflow.Api.Service
{
    public class VehicleService : BaseService
    {
        private IVehicleRepository _vehicleRepository;

        private IUserRepository _userRepository;

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

        public async Task<ResultModel> GetById(int id, int userId)
        {
            var p = await _vehicleRepository.GetById(id);
            return new ResultDataModel<Vehicle>(p?.UserId == userId ? p : null);
        }

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
    }
}
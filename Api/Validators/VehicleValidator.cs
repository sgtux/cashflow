using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class VehicleValidator : AbstractValidator<Vehicle>
    {
        private IVehicleRepository _vehicleRepository;

        private IUserRepository _userRepository;

        public VehicleValidator(IVehicleRepository creditCardRepository, IUserRepository userRepository)
        {
            _vehicleRepository = creditCardRepository;
            _userRepository = userRepository;
            RuleFor(c => c.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(c => c).Must(VehicleExists).When(c => c.Id > 0).WithMessage(ValidatorMessages.NotFound("Veículo"));
        }

        public bool VehicleExists(Vehicle vehicle)
        {
            return _vehicleRepository.GetById(vehicle.Id).Result != null;
        }
    }
}
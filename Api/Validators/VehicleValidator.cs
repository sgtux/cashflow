using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class VehicleValidator : AbstractValidator<Vehicle>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleValidator(IVehicleRepository vehicleRepository, IUserRepository userRepository)
        {
            _vehicleRepository = vehicleRepository;
            RuleFor(c => c.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(c => c.Description).MaximumLength(200).WithMessage(ValidatorMessages.FieldMaxLength("Descrição", 200));
            RuleFor(c => c).Must(VehicleExists).When(c => c.Id > 0).WithMessage(ValidatorMessages.NotFound("Veículo"));
        }

        public bool VehicleExists(Vehicle vehicle) => _vehicleRepository.GetById(vehicle.Id).Result?.UserId == vehicle.UserId;
    }
}
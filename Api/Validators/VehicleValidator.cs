using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class VehicleValidator : AbstractValidator<VehicleEntity>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleValidator(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
            RuleFor(c => c.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(c => c.Description).MaximumLength(200).WithMessage(ValidatorMessages.FieldMaxLength("Descrição", 200));
            RuleFor(c => c).MustAsync((vehicle, _) => VehicleExists(vehicle)).When(c => c.Id > 0).WithMessage(ValidatorMessages.NotFound("Veículo"));
        }

        public async Task<bool> VehicleExists(VehicleEntity vehicle) => (await _vehicleRepository.GetById(vehicle.Id))?.UserId == vehicle.UserId;
    }
}
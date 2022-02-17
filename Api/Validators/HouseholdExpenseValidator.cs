using Cashflow.Api.Contracts;
using Cashflow.Api.Enums;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class HouseholdExpenseValidator : AbstractValidator<HouseholdExpense>
    {
        private IVehicleRepository _vehicleRepository;

        public HouseholdExpenseValidator(IHouseholdExpenseRepository repository, IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
            RuleFor(s => s.Date).NotEqual(default(System.DateTime)).WithMessage(ValidatorMessages.FieldIsRequired("Data"));
            RuleFor(s => s.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(s => s.Value).GreaterThan(0).WithMessage(ValidatorMessages.GreaterThan("Valor", 0));
            RuleFor(s => s.Type).IsInEnum().WithMessage("Tipo inválido");
            RuleFor(c => c).Must(VehicleExists).WithMessage(ValidatorMessages.NotFound("Veículo"));
        }

        private bool VehicleExists(HouseholdExpense householdExpense)
        {
            if (householdExpense.VehicleId > 0)
            {
                var vehicle = _vehicleRepository.GetById(householdExpense.VehicleId.Value).Result;
                return vehicle?.UserId == householdExpense.UserId;
            }
            else
                householdExpense.VehicleId = null;
            return true;
        }
    }
}
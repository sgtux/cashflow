using System.Linq;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class HouseholdExpenseValidator : AbstractValidator<HouseholdExpense>
    {
        private IVehicleRepository _vehicleRepository;

        private ICreditCardRepository _creditCardRepository;

        public HouseholdExpenseValidator(IHouseholdExpenseRepository repository,
            IVehicleRepository vehicleRepository,
            ICreditCardRepository creditCardRepository)
        {
            _vehicleRepository = vehicleRepository;
            _creditCardRepository = creditCardRepository;
            RuleFor(s => s.Date).NotEqual(default(System.DateTime)).WithMessage(ValidatorMessages.FieldIsRequired("Data"));
            RuleFor(s => s.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(s => s.Value).GreaterThan(0).WithMessage(ValidatorMessages.GreaterThan("Valor", 0));
            RuleFor(s => s.Type).IsInEnum().WithMessage("Tipo inválido");
            RuleFor(c => c).Must(VehicleExists).WithMessage(ValidatorMessages.NotFound("Veículo"));
            RuleFor(c => c).Must(CreditCardExists).WithMessage(ValidatorMessages.NotFound("Cartão de Crédito"));
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

        private bool CreditCardExists(HouseholdExpense householdExpense)
        {
            if (householdExpense.CreditCardId > 0)
            {
                var cards = _creditCardRepository.GetSome(new BaseFilter() { UserId = householdExpense.UserId }).Result;
                return cards.Any(p => p.Id == householdExpense.CreditCardId);
            }
            else
                householdExpense.CreditCardId = null;
            return true;
        }
    }
}
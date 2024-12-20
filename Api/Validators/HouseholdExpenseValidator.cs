using System.Linq;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class HouseholdExpenseValidator : AbstractValidator<HouseholdExpense>
    {
        private readonly IVehicleRepository _vehicleRepository;

        private readonly ICreditCardRepository _creditCardRepository;

        public HouseholdExpenseValidator(IVehicleRepository vehicleRepository,
            ICreditCardRepository creditCardRepository)
        {
            _vehicleRepository = vehicleRepository;
            _creditCardRepository = creditCardRepository;
            RuleFor(s => s.Date).NotEqual(default(System.DateTime)).WithMessage(ValidatorMessages.FieldIsRequired("Data"));
            RuleFor(s => s.Description).NotEmpty().WithMessage(ValidatorMessages.FieldIsRequired("Descrição"));
            RuleFor(s => s.Value).GreaterThan(0).WithMessage(ValidatorMessages.GreaterThan("Valor", 0));
            RuleFor(s => s.Type).IsInEnum().WithMessage("Tipo inválido");
            RuleFor(c => c).Must(VehicleExists).WithMessage(ValidatorMessages.NotFound("Veículo"));
            RuleFor(p => p).Must(ValidCreditCard).WithMessage(ValidatorMessages.NotFound("Cartão de Crédito"));
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

        private bool ValidCreditCard(HouseholdExpense householdExpense)
        {
            if (householdExpense.CreditCardId > 0)
            {
                var card = _creditCardRepository.GetSome(new BaseFilter() { UserId = householdExpense.UserId }).Result.FirstOrDefault(p => p.Id == householdExpense.CreditCardId.Value);
                if (card == null || card.UserId != householdExpense.UserId)
                    return false;
            }
            else
                householdExpense.CreditCardId = null;
            return true;
        }
    }
}
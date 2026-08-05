using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class HouseholdExpenseValidator : AbstractValidator<HouseholdExpenseEntity>
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
            RuleFor(c => c).MustAsync((householdExpense, _) => VehicleExists(householdExpense)).WithMessage(ValidatorMessages.NotFound("Veículo"));
            RuleFor(p => p).MustAsync((householdExpense, _) => ValidCreditCard(householdExpense)).WithMessage(ValidatorMessages.NotFound("Cartão de Crédito"));
        }

        private async Task<bool> VehicleExists(HouseholdExpenseEntity householdExpense)
        {
            if (householdExpense.VehicleId > 0)
            {
                var vehicle = await _vehicleRepository.GetById(householdExpense.VehicleId.Value);
                return vehicle?.UserId == householdExpense.UserId;
            }
            else
                householdExpense.VehicleId = null;
            return true;
        }

        private async Task<bool> ValidCreditCard(HouseholdExpenseEntity householdExpense)
        {
            if (householdExpense.CreditCardId > 0)
            {
                var cards = await _creditCardRepository.GetSome(new BaseFilter() { UserId = householdExpense.UserId });
                var card = cards.FirstOrDefault(p => p.Id == householdExpense.CreditCardId.Value);
                if (card == null || card.UserId != householdExpense.UserId)
                    return false;
            }
            else
                householdExpense.CreditCardId = null;
            return true;
        }
    }
}
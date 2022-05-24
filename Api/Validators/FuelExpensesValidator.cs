using System;
using System.Linq;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class FuelExpensesValidator : AbstractValidator<FuelExpenses>
    {
        private IVehicleRepository _vehicleRepository;

        private IFuelExpensesRepository _fuelExpensesRepository;

        private ICreditCardRepository _creditCardRepository;

        private int _userId;

        public FuelExpensesValidator(IVehicleRepository vehicleRepository,
            IFuelExpensesRepository fuelExpensesRepository,
            ICreditCardRepository creditCardRepository,
            int userId)
        {
            _userId = userId;
            _vehicleRepository = vehicleRepository;
            _fuelExpensesRepository = fuelExpensesRepository;
            _creditCardRepository = creditCardRepository;
            RuleFor(c => c.Miliage).GreaterThan(0).WithMessage(ValidatorMessages.MinValue("Quilometragem", 0));
            RuleFor(c => c.Miliage).LessThan(1000000000).WithMessage(ValidatorMessages.MaxValue("Quilometragem", 999999999));
            RuleFor(c => c.PricePerLiter).GreaterThan(0).WithMessage(ValidatorMessages.MinValue("Preço por Litro", 0));
            RuleFor(c => c.PricePerLiter).LessThan(1000000000).WithMessage(ValidatorMessages.MaxValue("Preço por Litro", 999999999));
            RuleFor(c => c.ValueSupplied).GreaterThan(0).WithMessage(ValidatorMessages.MinValue("Valor Abastecido", 0));
            RuleFor(c => c.ValueSupplied).LessThan(1000000000).WithMessage(ValidatorMessages.MaxValue("Valor Abastecido", 999999999));
            RuleFor(c => c.Date).NotEqual(default(DateTime)).WithMessage(ValidatorMessages.FieldIsRequired("Data"));
            RuleFor(c => c).Must(VehicleExists).WithMessage(ValidatorMessages.NotFound("Veículo"));
            RuleFor(c => c).Must(FuelExpensesExists).When(c => c.Id > 0).WithMessage(ValidatorMessages.NotFound("Despesa de combustível"));
            RuleFor(c => c).Must(CreditCardExists).WithMessage(ValidatorMessages.NotFound("Cartão de Crédito"));
            RuleFor(c => c).Must(DataMiliageIsMatch).WithMessage("Data e Quilometragem não batem devido à outro abastecimento");
        }

        private bool VehicleExists(FuelExpenses fuelExpenses)
        {
            var vehicle = _vehicleRepository.GetById(fuelExpenses.VehicleId).Result;
            return vehicle?.UserId == _userId;
        }

        private bool FuelExpensesExists(FuelExpenses fuelExpenses) => _fuelExpensesRepository.GetById(fuelExpenses.Id).Result != null;

        private bool CreditCardExists(FuelExpenses fuelExpenses)
        {
            if (fuelExpenses.CreditCardId > 0)
            {
                var cards = _creditCardRepository.GetSome(new BaseFilter() { UserId = _userId }).Result;
                return cards.Any(p => p.Id == fuelExpenses.CreditCardId);
            }
            else
                fuelExpenses.CreditCardId = null;
            return true;
        }

        private bool DataMiliageIsMatch(FuelExpenses fuelExpense)
        {
            var vehicle = _vehicleRepository.GetById(fuelExpense.VehicleId).Result;
            if (vehicle == null)
                return false;
            return !vehicle.FuelExpenses.Any(p => p.Id != fuelExpense.Id && (fuelExpense.Miliage == p.Miliage
                || (fuelExpense.Miliage > p.Miliage && !IsSameDay(fuelExpense.Date, p.Date) && fuelExpense.Date < p.Date)
                || fuelExpense.Miliage < p.Miliage && !IsSameDay(fuelExpense.Date, p.Date) && fuelExpense.Date > p.Date));
        }

        private bool IsSameDay(DateTime date1, DateTime date2)
        {
            return date1.Day == date2.Day && date1.Month == date2.Month && date1.Year == date2.Year;
        }
    }
}
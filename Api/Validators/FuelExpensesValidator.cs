using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class FuelExpensesValidator : AbstractValidator<FuelExpenses>
    {
        private IVehicleRepository _vehicleRepository;

        private IFuelExpensesRepository _fuelExpensesRepository;

        private int _userId;

        public FuelExpensesValidator(IVehicleRepository vehicleRepository, IFuelExpensesRepository fuelExpensesRepository, int userId)
        {
            _userId = userId;
            _vehicleRepository = vehicleRepository;
            _fuelExpensesRepository = fuelExpensesRepository;
            RuleFor(c => c.Miliage).GreaterThan(0).WithMessage(ValidatorMessages.MinValue("Quilometragem", 0));
            RuleFor(c => c.Miliage).LessThan(10000000).WithMessage(ValidatorMessages.MaxValue("Quilometragem", 10000000));
            RuleFor(c => c.PricePerLiter).GreaterThan(0).WithMessage(ValidatorMessages.MinValue("Preço por Litro", 0));
            RuleFor(c => c.PricePerLiter).LessThan(1000).WithMessage(ValidatorMessages.MaxValue("Preço por Litro", 1000));
            RuleFor(c => c.ValueSupplied).GreaterThan(0).WithMessage(ValidatorMessages.MinValue("Valor Abastecido", 0));
            RuleFor(c => c.ValueSupplied).LessThan(1000).WithMessage(ValidatorMessages.MaxValue("Valor Abastecido", 1000));
            RuleFor(c => c).Must(VehicleExists).WithMessage(ValidatorMessages.NotFound("Veículo"));
            RuleFor(c => c).Must(FuelExpensesExists).When(c => c.Id > 0).WithMessage(ValidatorMessages.NotFound("Despesa de combustível"));
        }

        public bool VehicleExists(FuelExpenses fuelExpenses) => _vehicleRepository.GetById(fuelExpenses.VehicleId).Result?.UserId == _userId;

        public bool FuelExpensesExists(FuelExpenses fuelExpenses) => _fuelExpensesRepository.GetById(fuelExpenses.Id).Result != null;
    }
}
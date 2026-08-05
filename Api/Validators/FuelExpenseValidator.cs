using System;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using FluentValidation;

namespace Cashflow.Api.Validators
{
    public class FuelExpenseValidator : AbstractValidator<FuelExpenseEntity>
    {
        private readonly IVehicleRepository _vehicleRepository;

        private readonly IFuelExpenseRepository _fuelExpenseRepository;

        private int _userId;

        public FuelExpenseValidator(IVehicleRepository vehicleRepository,
            IFuelExpenseRepository fuelExpenseRepository,
            int userId)
        {
            _userId = userId;
            _vehicleRepository = vehicleRepository;
            _fuelExpenseRepository = fuelExpenseRepository;
            RuleFor(c => c.Miliage).GreaterThan(0).WithMessage(ValidatorMessages.MinValue("Quilometragem", 0));
            RuleFor(c => c.Miliage).LessThan(1000000000).WithMessage(ValidatorMessages.MaxValue("Quilometragem", 999999999));
            RuleFor(c => c.PricePerLiter).GreaterThan(0).WithMessage(ValidatorMessages.MinValue("Preço por Litro", 0));
            RuleFor(c => c.PricePerLiter).LessThan(1000000000).WithMessage(ValidatorMessages.MaxValue("Preço por Litro", 999999999));
            RuleFor(c => c.ValueSupplied).GreaterThan(0).WithMessage(ValidatorMessages.MinValue("Valor Abastecido", 0));
            RuleFor(c => c.ValueSupplied).LessThan(1000000000).WithMessage(ValidatorMessages.MaxValue("Valor Abastecido", 999999999));
            RuleFor(c => c.Date).NotEqual(default(DateTime)).WithMessage(ValidatorMessages.FieldIsRequired("Data"));
            RuleFor(c => c).MustAsync((fuelExpense, _) => VehicleExists(fuelExpense)).WithMessage(ValidatorMessages.NotFound("Veículo"));
            RuleFor(c => c).MustAsync((fuelExpense, _) => FuelExpenseExists(fuelExpense)).When(c => c.Id > 0).WithMessage(ValidatorMessages.NotFound("Despesa de combustível"));
            RuleFor(c => c).MustAsync((fuelExpense, _) => DataMiliageIsMatch(fuelExpense)).WithMessage("Data e Quilometragem não batem devido à outro abastecimento");
        }

        private Task<VehicleEntity> GetVehicle(int vehicleId) => _vehicleRepository.GetById(vehicleId);

        private async Task<bool> VehicleExists(FuelExpenseEntity fuelExpense)
        {
            var vehicle = await GetVehicle(fuelExpense.VehicleId);
            return vehicle?.UserId == _userId;
        }

        private async Task<bool> FuelExpenseExists(FuelExpenseEntity fuelExpense)
        {
            var existing = await _fuelExpenseRepository.GetById(fuelExpense.Id);
            if (existing == null)
                return false;
            var vehicle = await GetVehicle(existing.VehicleId);
            return vehicle?.UserId == _userId;
        }

        private async Task<bool> DataMiliageIsMatch(FuelExpenseEntity fuelExpense)
        {
            var vehicle = await GetVehicle(fuelExpense.VehicleId);
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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Infra.Sql.Vehicle;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Infra.Repository
{
    public class FuelExpensesRepository : BaseRepository<FuelExpenses>, IFuelExpensesRepository
    {
        private ICreditCardRepository _creditCardRepository;

        public FuelExpensesRepository(IDatabaseContext conn,
            LogService logService,
            ICreditCardRepository creditCardRepository) : base(conn, logService)
        {
            _creditCardRepository = creditCardRepository;
        }

        public Task Add(FuelExpenses t) => Execute(FuelExpensesResources.Insert, t);

        public async Task<FuelExpenses> GetById(long id)
        {
            var expense = await FirstOrDefault(FuelExpensesResources.ById, new { Id = id });
            if (expense != null && expense.CreditCardId.HasValue)
                expense.CreditCard = await _creditCardRepository.GetById(expense.CreditCardId.Value);
            return expense;
        }

        public Task Update(FuelExpenses t) => Execute(FuelExpensesResources.Update, t);

        public Task Remove(long id) => Execute(FuelExpensesResources.Delete, new { Id = id });

        public Task<IEnumerable<FuelExpenses>> GetSome(BaseFilter filter) => throw new NotImplementedException();
    }
}
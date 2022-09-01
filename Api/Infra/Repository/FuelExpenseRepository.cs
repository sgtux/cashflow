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
    public class FuelExpenseRepository : BaseRepository<FuelExpense>, IFuelExpenseRepository
    {
        private readonly ICreditCardRepository _creditCardRepository;

        public FuelExpenseRepository(IDatabaseContext conn,
            LogService logService,
            ICreditCardRepository creditCardRepository) : base(conn, logService)
        {
            _creditCardRepository = creditCardRepository;
        }

        public Task Add(FuelExpense t) => Execute(FuelExpenseResources.Insert, t);

        public async Task<FuelExpense> GetById(long id) => await FirstOrDefault(FuelExpenseResources.ById, new { Id = id });

        public Task Update(FuelExpense t) => Execute(FuelExpenseResources.Update, t);

        public Task Remove(long id) => Execute(FuelExpenseResources.Delete, new { Id = id });

        public Task<IEnumerable<FuelExpense>> GetSome(BaseFilter filter) => throw new NotImplementedException();
    }
}
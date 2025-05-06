using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IHouseholdExpenseRepository : IRepository<HouseholdExpense, HouseholdExpenseFilter> { }
}
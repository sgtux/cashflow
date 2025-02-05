using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Models.Account
{
    public class UserDataModel
    {
        public UserDataModel() { }

        public UserDataModel(User user)
        {
            Id = user.Id;
            Email = user.Email;
            ExpenseLimit = user.ExpenseLimit;
            FuelExpenseLimit = user.FuelExpenseLimit;
        }

        public int Id { get; set; }

        public string Email { get; set; }

        public decimal ExpenseLimit { get; set; }

        public decimal FuelExpenseLimit { get; set; }
    }
}
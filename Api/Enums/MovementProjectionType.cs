namespace Cashflow.Api.Enums
{
    public enum MovementProjectionType : byte
    {
        Earning = 0,

        Payment = 1,

        FuelExpense = 2,

        HouseholdExpense = 3,

        RecurringExpenses = 4,
        
        RemainingBalanceIn = 5,

        RemainingBalanceOut = 6
    }
}
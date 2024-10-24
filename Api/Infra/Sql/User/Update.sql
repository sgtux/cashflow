UPDATE
    [User]
SET
    ExpenseLimit = @ExpenseLimit,
    FuelExpenseLimit = @FuelExpenseLimit
WHERE
    Id = @Id
UPDATE
    [User]
SET
    ExpenseLimit = @ExpenseLimit,
    FuelExpenseLimit = @FuelExpenseLimit,
    RecordsUsed = @RecordsUsed
WHERE
    Id = @Id
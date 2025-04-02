SELECT 
    Id,
    Email,
    Password,
    CreatedAt,
    ExpenseLimit,
    FuelExpenseLimit,
    [Plan],
    RecordsUsed
FROM [User]
WHERE Email = @Email
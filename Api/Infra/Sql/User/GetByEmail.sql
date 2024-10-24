SELECT 
    Id,
    Email,
    Password,
    CreatedAt,
    ExpenseLimit,
    FuelExpenseLimit
FROM [User]
WHERE Email = @Email
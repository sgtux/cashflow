SELECT 
    Id,
    Email,
    Password,
    CreatedAt,
    SpendingCeiling
FROM [User]
WHERE Id = @Id
SELECT 
    Id,
    Email,
    Password,
    CreatedAt,
    SpendingCeiling
FROM User
WHERE Email = @Email
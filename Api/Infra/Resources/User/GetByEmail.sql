SELECT 
    "Id",
    "Name",
    "Email",
    "Password",
    "CreatedAt",
    "UpdatedAt"
FROM "User"
WHERE "Email" = @Email
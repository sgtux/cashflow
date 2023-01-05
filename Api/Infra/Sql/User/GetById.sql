SELECT 
    "Id",
    "NickName",
    "Password",
    "CreatedAt",
    "SpendingCeiling"
FROM "User"
WHERE "Id" = @Id
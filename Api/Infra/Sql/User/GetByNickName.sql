SELECT 
    "Id",
    "NickName",
    "Password",
    "CreatedAt",
    "SpendingCeiling"
FROM "User"
WHERE "NickName" = @NickName
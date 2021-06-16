SELECT 
    "Id",
    "NickName",
    "Password",
    "CreatedAt"
FROM "User"
WHERE "NickName" = @NickName
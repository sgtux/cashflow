SELECT 
    "Id",
    "Description",
    "UserId"
FROM "Vehicle"
WHERE "UserId" = @UserId
ORDER BY "Id"
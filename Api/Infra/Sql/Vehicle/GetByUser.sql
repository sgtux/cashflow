SELECT 
    v."Id",
    v."Description",
    v."UserId",
    f."Id",
    f."Miliage",
    f."ValueSupplied",
    f."PricePerLiter",
    f."VehicleId"
FROM "Vehicle" v
LEFT JOIN "FuelExpenses" f ON v."Id" = f."VehicleId"
WHERE "UserId" = @UserId
ORDER BY v."Id", f."Id"
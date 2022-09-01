SELECT
    v."Id",
    v."Description",
    v."UserId",
    f."Id",
    f."Miliage",
    f."ValueSupplied",
    f."PricePerLiter",
    f."VehicleId",
    f."Date"
FROM
    "Vehicle" v
    LEFT JOIN "FuelExpense" f ON v."Id" = f."VehicleId"
    AND (
        @StartDate IS NULL
        OR f."Date" >= @StartDate
    )
    AND (
        @EndDate IS NULL
        OR f."Date" <= @EndDate
    )
WHERE
    "UserId" = @UserId
ORDER BY
    v."Id",
    f."Date",
    f."Miliage"
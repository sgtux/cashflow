SELECT
    v."Id",
    v."Description",
    v."UserId",
    f."Id",
    f."Miliage",
    f."ValueSupplied",
    f."PricePerLiter",
    f."VehicleId",
    f."Date",
    f."CreditCardId"
FROM
    "Vehicle" v
    LEFT JOIN "FuelExpenses" f ON v."Id" = f."VehicleId"
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
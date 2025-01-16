SELECT
    v.Id,
    v.Description,
    v.UserId,
    v.Active,
    f.Id,
    f.Miliage,
    f.ValueSupplied,
    f.PricePerLiter,
    f.VehicleId,
    f.Date
FROM
    Vehicle v
    LEFT JOIN FuelExpense f ON v.Id = f.VehicleId
    AND (
        @StartDate IS NULL
        OR f.Date >= @StartDate
    )
    AND (
        @EndDate IS NULL
        OR f.Date <= @EndDate
    )
WHERE
    v.UserId = @UserId
    AND (@Active IS NULL OR v.Active = @Active)
ORDER BY
    v.Id ASC,
    f.Date DESC,
    f.Miliage ASC
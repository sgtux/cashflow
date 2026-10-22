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
    f.CreditCardId,
    f.Date,
    c.Id,
    c.Name,
    c.InvoiceClosingDay,
    c.InvoiceDueDay
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
    LEFT JOIN CreditCard c ON c.Id = f.CreditCardId
WHERE
    v.UserId = @UserId
    AND (@Active IS NULL OR v.Active = @Active)
ORDER BY
    v.Id ASC,
    f.Date DESC,
    f.Miliage ASC
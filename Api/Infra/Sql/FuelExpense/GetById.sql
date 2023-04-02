SELECT
    Miliage,
    ValueSupplied,
    PricePerLiter,
    VehicleId,
    Date
FROM
    FuelExpense
WHERE
    Id = @Id
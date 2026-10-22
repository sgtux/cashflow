SELECT
    Miliage,
    ValueSupplied,
    PricePerLiter,
    VehicleId,
    CreditCardId,
    Date
FROM
    FuelExpense
WHERE
    Id = @Id
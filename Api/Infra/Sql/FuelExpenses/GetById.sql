SELECT
    "Miliage",
    "ValueSupplied",
    "PricePerLiter",
    "VehicleId",
    "Date",
    "CreditCardId"
FROM
    "FuelExpenses"
WHERE
    "Id" = @Id
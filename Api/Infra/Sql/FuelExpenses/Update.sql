UPDATE "FuelExpenses"
SET
  "Miliage" = @Miliage,
  "ValueSupplied" = @ValueSupplied,
  "PricePerLiter" = @PricePerLiter,
  "VehicleId" = @VehicleId,
  "Date" = @Date,
  "CreditCardId" = @CreditCardId
WHERE
  "Id" = @Id
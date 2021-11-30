UPDATE "FuelExpenses"
SET
  "Miliage" = @Miliage,
  "ValueSupplied" = @ValueSupplied,
  "PricePerLiter" = @PricePerLiter,
  "VehicleId" = @VehicleId,
  "Date" = @Date
WHERE
  "Id" = @Id
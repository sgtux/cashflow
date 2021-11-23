UPDATE "FuelExpenses"
SET
  "Miliage" = @Miliage,
  "ValueSupplied" = @ValueSupplied,
  "PricePerLiter" = @PricePerLiter,
  "VehicleId" = @VehicleId 
WHERE
  "Id" = @Id
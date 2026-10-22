UPDATE FuelExpense
SET
  Miliage = @Miliage,
  ValueSupplied = @ValueSupplied,
  PricePerLiter = @PricePerLiter,
  VehicleId = @VehicleId,
  CreditCardId = @CreditCardId,
  Date = @Date
WHERE
  Id = @Id
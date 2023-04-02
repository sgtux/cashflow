UPDATE
  HouseholdExpense
SET
  Description = @Description,
  Date = @Date,
  Value = @Value,
  VehicleId = @VehicleId,
  Type = @Type
WHERE
  Id = @Id
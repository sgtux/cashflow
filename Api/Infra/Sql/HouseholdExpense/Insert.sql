INSERT INTO
  HouseholdExpense (
    Description,
    Date,
    UserId,
    Value,
    VehicleId,
    Type
  )
VALUES
  (
    @Description,
    @Date,
    @UserId,
    @Value,
    @VehicleId,
    @Type
  )
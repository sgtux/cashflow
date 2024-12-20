INSERT INTO
  HouseholdExpense (
    Description,
    Date,
    UserId,
    Value,
    VehicleId,
    Type,
    CreditCardId
  )
VALUES
  (
    @Description,
    @Date,
    @UserId,
    @Value,
    @VehicleId,
    @Type,
    @CreditCardId
  )
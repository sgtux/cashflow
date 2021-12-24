INSERT INTO
  "Payment" (
    "Description",
    "UserId",
    "Type",
    "CreditCardId",
    "Condition",
    "InactiveAt",
    "BaseCost",
    "Date"
  )
VALUES
  (
    @Description,
    @UserId,
    @TypeId,
    @CreditCardId,
    @Condition,
    @InactiveAt,
    @BaseCost,
    @Date
  )
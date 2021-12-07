INSERT INTO
  "Payment" (
    "Description",
    "UserId",
    "Type",
    "CreditCardId",
    "Condition",
    "InactiveAt"
  )
VALUES
  (
    @Description,
    @UserId,
    @TypeId,
    @CreditCardId,
    @Condition,
    @InactiveAt
  )
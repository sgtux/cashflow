INSERT INTO
  "Payment" (
    "Description",
    "UserId",
    "Type",
    "CreditCardId",
    "Date"
  )
VALUES
  (
    @Description,
    @UserId,
    @TypeId,
    @CreditCardId,
    @Date
  )
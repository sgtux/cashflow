INSERT INTO
  "Payment" (
    "Description",
    "UserId",
    "Type",
    "CreditCardId",
    "InactiveAt",
    "Date"
  )
VALUES
  (
    @Description,
    @UserId,
    @TypeId,
    @CreditCardId,
    @InactiveAt,
    @Date
  )
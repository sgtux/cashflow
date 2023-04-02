INSERT INTO
  Payment (
    Description,
    UserId,
    Type,
    CreditCardId,
    Date
  )
VALUES
  (
    @Description,
    @UserId,
    @Type,
    @CreditCardId,
    @Date
  )
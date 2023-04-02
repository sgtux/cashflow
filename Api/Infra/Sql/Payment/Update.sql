UPDATE
  Payment
SET
  Description = @Description,
  UserId = @UserId,
  Type = @Type,
  CreditCardId = @CreditCardId,
  Date = @Date
WHERE
  Id = @Id
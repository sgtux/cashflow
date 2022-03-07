UPDATE
  "Payment"
SET
  "Description" = @Description,
  "UserId" = @UserId,
  "Type" = @TypeId,
  "CreditCardId" = @CreditCardId,
  "Date" = @Date
WHERE
  "Id" = @Id
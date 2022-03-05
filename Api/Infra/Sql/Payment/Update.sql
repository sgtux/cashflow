UPDATE
  "Payment"
SET
  "Description" = @Description,
  "UserId" = @UserId,
  "Type" = @TypeId,
  "CreditCardId" = @CreditCardId,
  "InactiveAt" = @InactiveAt,
  "Date" = @Date
WHERE
  "Id" = @Id
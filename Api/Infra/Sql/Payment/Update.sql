UPDATE
  "Payment"
SET
  "Description" = @Description,
  "UserId" = @UserId,
  "Type" = @TypeId,
  "CreditCardId" = @CreditCardId,
  "Condition" = @Condition,
  "InactiveAt" = @InactiveAt,
  "BaseCost" = @BaseCost,
  "Date" = @Date
WHERE
  "Id" = @Id
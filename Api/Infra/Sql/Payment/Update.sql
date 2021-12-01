UPDATE "Payment" 
SET 
  "Description" = @Description,
  "UserId" = @UserId,
  "Type" = @TypeId,
  "CreditCardId" = @CreditCardId,
  "Condition" = @Condition,
  "InactiveAt" = @InactiveAt
WHERE "Id" = @Id
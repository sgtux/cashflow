UPDATE "Payment" 
SET 
  "Description" = @Description,
  "UserId" = @UserId,
  "Type" = @TypeId,
  "CreditCardId" = @CreditCardId,
  "Condition" = @Condition,
  "Active" = @Active
WHERE "Id" = @Id
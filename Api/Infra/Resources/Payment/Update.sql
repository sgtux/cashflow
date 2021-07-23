UPDATE "Payment" 
SET 
  "Description" = @Description,
  "UserId" = @UserId,
  "Type" = @TypeId,
  "CreditCardId" = @CreditCardId,
  "Condition" = @Condition,
  "Invoice" = @Invoice
WHERE "Id" = @Id
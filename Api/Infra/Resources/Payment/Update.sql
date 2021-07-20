UPDATE "Payment" 
SET 
  "Description" = @Description,
  "UserId" = @UserId,
  "Type" = @TypeId,
  "CreditCardId" = @CreditCardId,
  "Paid" = @Paid,
  "Condition" = @Condition,
  "Invoice" = @Invoice
WHERE "Id" = @Id
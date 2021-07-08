UPDATE "Payment" 
SET 
  "Description" = @Description,
  "UserId" = @UserId,
  "Type" = @TypeId,
  "CreditCardId" = @CreditCardId,
  "FixedPayment" = @FixedPayment,
  "Invoice" = @Invoice
WHERE "Id" = @Id
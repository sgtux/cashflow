UPDATE "Payment" 
SET 
  "Description" = @Description,
  "UserId" = @UserId,
  "Type" = @Type,
  "CreditCardId" = @CreditCardId,
  "FixedPayment" = @FixedPayment,
  "Invoice" = @Invoice
WHERE "Id" = @Id
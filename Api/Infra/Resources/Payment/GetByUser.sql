SELECT 
  "Id", 
  "Description", 
  "UserId", 
  "Type", 
  "CreditCardId", 
  "FixedPayment", 
  "Invoice"
FROM "Payment"
WHERE "UserId" = @UserId
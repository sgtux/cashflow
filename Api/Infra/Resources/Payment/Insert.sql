INSERT INTO "Payment" 
  ("Description", "UserId", "Type", "CreditCardId", "FixedPayment", "Invoice")
VALUES
  (@Description, @UserId, @TypeId, @CreditCardId, @FixedPayment, @Invoice)
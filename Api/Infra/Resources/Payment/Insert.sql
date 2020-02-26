INSERT INTO "Payment" 
  ("Description", "UserId", "Type", "CreditCardId", "FixedPayment", "Invoice")
VALUES
  (@Description, @UserId, @Type, @CreditCardId, @FixedPayment, @Invoice)
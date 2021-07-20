INSERT INTO "Payment" 
  ("Description", "UserId", "Type", "CreditCardId", "Paid", "Condition", "Invoice")
VALUES
  (@Description, @UserId, @TypeId, @CreditCardId, @Paid, @Condition, @Invoice)
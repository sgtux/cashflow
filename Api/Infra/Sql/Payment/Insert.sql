INSERT INTO "Payment" 
  ("Description", "UserId", "Type", "CreditCardId", "Condition", "Invoice")
VALUES
  (@Description, @UserId, @TypeId, @CreditCardId, @Condition, @Invoice)
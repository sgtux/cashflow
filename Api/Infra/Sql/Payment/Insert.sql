INSERT INTO "Payment" 
  ("Description", "UserId", "Type", "CreditCardId", "Condition", "Active")
VALUES
  (@Description, @UserId, @TypeId, @CreditCardId, @Condition, @Active)
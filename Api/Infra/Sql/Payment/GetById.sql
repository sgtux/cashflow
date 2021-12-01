SELECT 
  p."Id",
  p."Description",
  p."UserId",
  p."Type" "TypeId",
  p."CreditCardId",
  p."Condition",
  p."InactiveAt",
  i."Id",
  i."PaymentId",
  i."Cost",
  i."Number",
  i."Date",
  i."PaidDate"
FROM "Payment" p
JOIN "Installment" i on p."Id" = i."PaymentId"
WHERE p."Id" = @Id
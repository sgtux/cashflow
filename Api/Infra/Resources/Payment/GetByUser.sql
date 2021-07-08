SELECT 
  p."Id",
  p."Description",
  p."UserId",
  p."Type" "TypeId",
  p."CreditCardId",
  p."FixedPayment",
  p."Invoice",
  i."Id",
  i."PaymentId",
  i."Cost",
  i."Number",
  i."Date",
  i."PaidDate"
FROM "Payment" p
JOIN "Installment" i ON p."Id" = i."PaymentId"
JOIN "PaymentType" t ON t."Id" = p."Type"
WHERE p."UserId" = @UserId
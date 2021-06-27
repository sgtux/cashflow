SELECT 
  p."Id",
  p."Description",
  p."UserId",
  p."Type",
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
JOIN "Installment" i on p."Id" = i."PaymentId"
WHERE p."Id" = @Id
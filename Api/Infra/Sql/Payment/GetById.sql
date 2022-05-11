SELECT
  p."Id",
  p."Description",
  p."UserId",
  p."Type" "TypeId",
  p."CreditCardId",
  i."Id",
  i."PaymentId",
  i."Value",
  i."PaidValue",
  i."Number",
  i."Date",
  i."PaidDate",
  i."Exempt"
FROM
  "Payment" p
  JOIN "Installment" i ON p."Id" = i."PaymentId"
WHERE
  p."Id" = @Id
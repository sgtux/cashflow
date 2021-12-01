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
FROM
  "Payment" p
  JOIN "Installment" i ON p."Id" = i."PaymentId"
  AND (
    @StartDate IS NULL
    OR i."Date" >= @StartDate
  )
  AND (
    @EndDate IS NULL
    OR i."Date" <= @EndDate
  )
WHERE
  p."UserId" = @UserId
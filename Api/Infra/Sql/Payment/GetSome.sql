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
  p."UserId" = @UserId
  AND (
    @Description IS NULL
    OR LOWER(p."Description") LIKE LOWER(@Description)
  )
  AND (
    @StartDate IS NULL
    OR EXISTS (
      SELECT
        NULL
      FROM
        "Installment"
      WHERE
        "PaymentId" = p."Id"
        AND "Date" >= @StartDate
    )
  )
  AND (
    @EndDate IS NULL
    OR EXISTS (
      SELECT
        NULL
      FROM
        "Installment"
      WHERE
        "PaymentId" = p."Id"
        AND "Date" <= @EndDate
    )
  )
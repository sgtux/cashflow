SELECT
  p."Id",
  p."Description",
  p."UserId",
  p."Type" "TypeId",
  p."CreditCardId",
  p."Condition",
  p."InactiveAt",
  p."BaseCost",
  i."Id",
  i."PaymentId",
  i."Cost",
  i."Number",
  i."Date",
  i."PaidDate"
FROM
  "Payment" p
  JOIN "Installment" i ON p."Id" = i."PaymentId"
WHERE
  p."UserId" = @UserId
  AND (
    @StartDate IS NULL
    OR i."Date" >= @StartDate
  )
  AND (
    @EndDate IS NULL
    OR i."Date" <= @EndDate
  )
  AND (
    @Active IS NULL
    OR (
      (
        @Active = TRUE
        AND p."InactiveAt" IS NULL
      )
      OR (
        @Active = false
        AND p."InactiveAt" IS NOT NULL
      )
    )
  )
  AND (
    p."InactiveAt" IS NULL
    OR (
      @InactiveFrom IS NULL
      OR p."InactiveAt" >= @InactiveFrom
    )
  )
  AND(
    p."InactiveAt" IS NULL
    OR (
      @InactiveTo IS NULL
      OR p."InactiveAt" <= @InactiveTo
    )
  )
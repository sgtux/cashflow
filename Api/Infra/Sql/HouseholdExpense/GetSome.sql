SELECT
  p."Id",
  p."Description",
  p."Date",
  p."UserId",
  p."Value"
FROM
  "HouseholdExpense" p
WHERE
  p."UserId" = @UserId
  AND (
    @StartDate IS NULL
    OR p."Date" >= @StartDate
  )
  AND (
    @EndDate IS NULL
    OR p."Date" <= @EndDate
  )
ORDER BY
  p."Date"
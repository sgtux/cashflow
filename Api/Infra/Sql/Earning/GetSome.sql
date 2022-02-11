SELECT
  "Id",
  "Description",
  "Value",
  "Date",
  "UserId",
  "Type"
FROM
  "Earning"
WHERE
  "UserId" = @UserId
  AND (
    @StartDate IS NULL
    OR "Date" >= @StartDate
  )
  AND (
    @EndDate IS NULL
    OR "Date" <= @EndDate
  )
ORDER BY
  "Date"
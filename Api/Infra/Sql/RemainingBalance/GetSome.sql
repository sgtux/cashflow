SELECT
  "Id",
  "Value",
  "Month",
  "Year",
  "UserId"
FROM
  "RemainingBalance"
WHERE
  "UserId" = @UserId
ORDER BY
  "Year" DESC,
  "Month" DESC
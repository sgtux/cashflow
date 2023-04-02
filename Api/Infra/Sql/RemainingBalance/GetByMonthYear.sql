SELECT
  Id,
  Value,
  Month,
  Year,
  UserId
FROM
  RemainingBalance
WHERE
  UserId = @UserId
  AND Month = @Month
  AND Year = @Year
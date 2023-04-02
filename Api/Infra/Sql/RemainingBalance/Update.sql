UPDATE
  RemainingBalance
SET
  Value = @Value,
  Month = @Month,
  Year = @Year,
  UserId = @UserId
WHERE
  Id = @Id
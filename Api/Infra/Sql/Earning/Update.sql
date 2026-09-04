UPDATE
  Earning
SET
  Description = @Description,
  Value = @Value,
  Date = @Date
WHERE
  Id = @Id
  AND UserId = @UserId

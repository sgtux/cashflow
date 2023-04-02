UPDATE
  Earning
SET
  Description = @Description,
  Value = @Value,
  Date = @Date,
  Type = @Type
WHERE
  Id = @Id
  AND UserId = @UserId
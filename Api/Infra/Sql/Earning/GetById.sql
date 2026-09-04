SELECT
  Id,
  Description,
  Value,
  Date,
  UserId
FROM
  Earning
WHERE
  Id = @Id

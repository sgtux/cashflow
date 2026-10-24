SELECT
  Id,
  Description,
  Value,
  Date,
  Type,
  UserId
FROM
  Earning
WHERE
  Id = @Id

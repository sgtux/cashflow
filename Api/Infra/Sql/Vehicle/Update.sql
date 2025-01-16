UPDATE Vehicle
SET
  Description = @Description,
  Active = @Active
WHERE
  Id = @Id
UPDATE
  "Salary"
SET
  "Value" = @Value,
  "StartDate" = @StartDate,
  "EndDate" = @EndDate
WHERE
  "Id" = @Id
  AND "UserId" = @UserId
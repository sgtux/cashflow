SELECt 
  "Id",
  "Value",
  "StartDate",
  "EndDate",
  "UserId"
FROM "Salary"
WHERE "Id" = @Id
ORDER BY "StartDate"
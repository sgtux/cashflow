SELECt 
  "Id",
  "Value",
  "StartDate",
  "EndDate",
  "UserId"
FROM "Salary"
WHERE "UserId" = @UserId
ORDER BY "StartDate"
UPDATE "DailyExpenses"
SET
  "DailyName" = @DailyName,
  "Date" = @Date
WHERE "Id" = @Id
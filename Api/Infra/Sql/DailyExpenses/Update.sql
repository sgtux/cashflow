UPDATE "DailyExpenses"
SET
  "ShopName" = @ShopName,
  "Date" = @Date
WHERE "Id" = @Id
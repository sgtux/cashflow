SELECT 
  p."Id",
  p."DailyName",
  p."Date",
  p."UserId",
  i."Id",
  i."ItemName",
  i."Price"
FROM "DailyExpenses" p
JOIN "DailyExpensesItems" i ON p."Id" = i."DailyExpensesId"
WHERE p."Id" = @Id
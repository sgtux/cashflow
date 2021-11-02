SELECT 
  p."Id",
  p."ShopName",
  p."Date",
  p."UserId",
  i."Id",
  i."ItemName",
  i."Price",
  i."Amount",
  i."DailyExpensesId"
FROM "DailyExpenses" p
JOIN "DailyExpensesItem" i ON p."Id" = i."DailyExpensesId"
WHERE p."UserId" = @UserId
ORDER BY p."Date"
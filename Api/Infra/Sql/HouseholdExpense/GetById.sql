SELECT
  p."Id",
  p."Description",
  p."Date",
  p."UserId",
  p."Value"
FROM
  "HouseholdExpense" p
WHERE
  p."Id" = @Id
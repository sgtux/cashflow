UPDATE
  "HouseholdExpense"
SET
  "Description" = @Description,
  "Date" = @Date,
  "Value" = @Value
WHERE
  "Id" = @Id
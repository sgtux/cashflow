UPDATE
  "HouseholdExpense"
SET
  "Description" = @Description,
  "Date" = @Date,
  "Value" = @Value,
  "VehicleId" = @VehicleId,
  "Type" = @Type,
  "CreditCardId" = @CreditCardId
WHERE
  "Id" = @Id
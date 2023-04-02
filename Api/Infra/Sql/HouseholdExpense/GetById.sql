SELECT
  p.Id,
  p.Description,
  p.Date,
  p.UserId,
  p.Value,
  p.VehicleId,
  p.Type
FROM
  HouseholdExpense p
WHERE
  p.Id = @Id
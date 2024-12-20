SELECT
  p.Id,
  p.Description,
  p.Date,
  p.UserId,
  p.Value,
  p.VehicleId,
  p.Type,
  p.CreditCardId,
  c.Id AS CreditCard_Id,
  c.Name AS CreditCard_Name,
  c.InvoiceClosingDay AS CreditCard_InvoiceClosingDay,
  c.InvoiceDueDay AS CreditCard_InvoiceDueDay
FROM
  HouseholdExpense p
  LEFT JOIN CreditCard c ON c.Id = p.CreditCardId
WHERE
  p.UserId = @UserId
  AND (
    @StartDate IS NULL
    OR p.Date >= @StartDate
  )
  AND (
    @EndDate IS NULL
    OR p.Date <= @EndDate
  )
ORDER BY
  p.Date
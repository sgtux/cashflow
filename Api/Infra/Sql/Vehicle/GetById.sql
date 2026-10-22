SELECT 
    v.Id,
    v.Description,
    v.UserId,
    v.Active,
    f.Id,
    f.Miliage,
    f.ValueSupplied,
    f.PricePerLiter,
    f.VehicleId,
    f.CreditCardId,
    f.Date,
    c.Id,
    c.Name,
    c.InvoiceClosingDay,
    c.InvoiceDueDay
FROM Vehicle v
LEFT JOIN FuelExpense f ON v.Id = f.VehicleId
LEFT JOIN CreditCard c ON c.Id = f.CreditCardId
WHERE v.Id = @Id
ORDER BY f.Date DESC, f.Miliage ASC
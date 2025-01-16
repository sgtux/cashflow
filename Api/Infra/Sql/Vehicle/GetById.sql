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
    f.Date
FROM Vehicle v
LEFT JOIN FuelExpense f ON v.Id = f.VehicleId
WHERE v.Id = @Id
ORDER BY f.Date DESC, f.Miliage ASC
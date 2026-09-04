SELECT
    re.Id,
    re.Description,
    re.Value,
    re.InactiveAt,
    re.UserId,
    reh.Id,
    reh.Value,
    reh.Date,
    reh.RecurringEarningId
FROM
    RecurringEarning re
    LEFT JOIN RecurringEarningHistory reh ON re.Id = reh.RecurringEarningId
WHERE
    re.Id = @Id

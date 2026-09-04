UPDATE
    RecurringEarningHistory
SET
    Value = @Value,
    Date = @Date
WHERE
    Id = @Id

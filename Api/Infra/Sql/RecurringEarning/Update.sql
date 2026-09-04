UPDATE
    RecurringEarning
SET
    Description = @Description,
    Value = @Value,
    InactiveAt = @InactiveAt
WHERE
    Id = @Id

UPDATE
    RecurringEarning
SET
    Description = @Description,
    Value = @Value,
    InactiveAt = @InactiveAt,
    Type = @Type
WHERE
    Id = @Id

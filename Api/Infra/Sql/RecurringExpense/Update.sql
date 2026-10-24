UPDATE
    RecurringExpense
SET
    Description = @Description,
    Value = @Value,
    InactiveAt = @InactiveAt,
    CreditCardId = @CreditCardId,
    Type = @Type
WHERE
    Id = @Id
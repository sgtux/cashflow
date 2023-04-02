UPDATE
    RecurringExpense
SET
    Description = @Description,
    Value = @Value,
    InactiveAt = @InactiveAt,
    CreditCardId = @CreditCardId
WHERE
    Id = @Id
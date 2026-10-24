INSERT INTO
    RecurringExpense (
        Description,
        Value,
        InactiveAt,
        CreditCardId,
        UserId,
        Type
    )
VALUES
    (
        @Description,
        @Value,
        @InactiveAt,
        @CreditCardId,
        @UserId,
        @Type
    )
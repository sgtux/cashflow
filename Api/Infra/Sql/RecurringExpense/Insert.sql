INSERT INTO
    "RecurringExpense" (
        "Description",
        "Value",
        "InactiveAt",
        "CreditCardId"
    )
VALUES
    (
        @Description,
        @Value,
        @InactiveAt,
        @CreditCardId
    )
INSERT INTO
    "RecurringExpense" (
        "Description",
        "Value",
        "InactiveAt",
        "CreditCardId",
        "UserId"
    )
VALUES
    (
        @Description,
        @Value,
        @InactiveAt,
        @CreditCardId,
        @UserId
    )
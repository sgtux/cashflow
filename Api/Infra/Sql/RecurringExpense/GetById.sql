SELECT
    re."Id",
    re."Description",
    re."Value",
    re."InactiveAt",
    re."CreditCardId",
    re."UserId",
    reh."Id",
    reh."PaidValue",
    reh."Date",
    reh."RecurringExpenseId"
FROM
    "RecurringExpense" re
    LEFT JOIN "RecurringExpenseHistory" reh ON re."Id" = reh."RecurringExpenseId"
WHERE
    re."Id" = @Id
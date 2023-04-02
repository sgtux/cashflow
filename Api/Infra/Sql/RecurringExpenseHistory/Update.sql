UPDATE
    RecurringExpenseHistory
SET
    PaidValue = @PaidValue,
    Date = @Date
WHERE
    Id = @Id
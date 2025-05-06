SELECT
	re.Id,
	re.Description,
	re.Value,
	re.InactiveAt,
	re.CreditCardId,
	re.UserId,
	reh.Id,
	reh.PaidValue,
	reh.Date,
	reh.RecurringExpenseId
FROM
	RecurringExpense re
	LEFT JOIN RecurringExpenseHistory reh ON re.Id = reh.RecurringExpenseId
WHERE
	re.UserId = @UserId
	AND (
		@CreditCardIdsStr IS NULL
		OR re.CreditCardId IN @CreditCardIds
	)
	AND (
		@Active IS NULL
		OR (
			(
				@Active = 1
				AND re.InactiveAt IS NULL
			)
			OR (
				@Active = 0
				AND re.InactiveAt IS NOT NULL
			)
		)
	)
	AND (
		@StartDate IS NULL
		OR reh.Date >= @StartDate
	)
	AND (
		@EndDate IS NULL
		OR reh.Date <= @EndDate
	)
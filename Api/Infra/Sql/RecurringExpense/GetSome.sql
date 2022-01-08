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
	re."UserId" = @UserId
	AND (
		@Active IS NULL
		OR (
			(
				@Active = TRUE
				AND re."InactiveAt" IS NULL
			)
			OR (
				@Active = FALSE
				AND re."InactiveAt" IS NOT NULL
			)
		)
	)
	AND (
		@StartDate IS NULL
		OR reh."Date" >= @StartDate
	)
	AND (
		@EndDate IS NULL
		OR reh."Date" <= @EndDate
	)
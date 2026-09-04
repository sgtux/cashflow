SELECT
	re.Id,
	re.Description,
	re.Value,
	re.InactiveAt,
	re.UserId,
	reh.Id,
	reh.Value,
	reh.Date,
	reh.RecurringEarningId
FROM
	RecurringEarning re
	LEFT JOIN RecurringEarningHistory reh ON re.Id = reh.RecurringEarningId
WHERE
	re.UserId = @UserId
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

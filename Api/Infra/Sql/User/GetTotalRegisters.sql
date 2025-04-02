SELECT
    SUM(total) total
FROM
    (
        SELECT
            COUNT(1) total
        FROM
            CreditCard
        WHERE
            UserId = @UserId
        UNION
        SELECT
            COUNT(1)
        FROM
            Earning
        WHERE
            UserId = @UserId
        UNION
        SELECT
            COUNT(1)
        FROM
            CreditCard
        WHERE
            UserId = @UserId
        UNION
        SELECT
            COUNT(1)
        FROM
            FuelExpense fe
            JOIN Vehicle v ON fe.VehicleId = v.Id
        WHERE
            v.UserId = @UserId
        UNION
        SELECT
            COUNT(1)
        FROM
            Vehicle
        WHERE
            UserId = @UserId
        UNION
        SELECT
            COUNT(1)
        FROM
            HouseholdExpense
        WHERE
            UserId = @UserId
        UNION
        SELECT
            COUNT(1)
        FROM
            Installment i
            JOIN Payment p ON i.PaymentId = p.Id
        WHERE
            p.UserId = @UserId
        UNION
        SELECT
            COUNT(1)
        FROM
            Payment
        WHERE
            UserId = @UserId
        UNION
        SELECT
            COUNT(1)
        FROM
            RecurringExpenseHistory reh
            JOIN RecurringExpense re ON reh.RecurringExpenseId = re.Id
        WHERE
            re.UserId = @UserId
        UNION
        SELECT
            COUNT(1)
        FROM
            RecurringExpense
        WHERE
            UserId = @UserId
        UNION
        SELECT
            COUNT(1)
        FROM
            RemainingBalance
        WHERE
            UserId = @UserId
    ) AS totals
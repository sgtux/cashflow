SELECT
  Id,
  Name,
  UserId,
  InvoiceClosingDay,
  InvoiceDueDay
FROM
  CreditCard
WHERE
  UserId = @UserId
ORDER BY
  Name
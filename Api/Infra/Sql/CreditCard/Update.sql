UPDATE
  "CreditCard"
SET
  "Name" = @Name,
  "UserId" = @UserId,
  "InvoiceClosingDay" = @InvoiceClosingDay,
  "InvoiceDueDay" = @InvoiceDueDay
WHERE
  "Id" = @Id
SELECT
  "Id",
  "Name",
  "UserId",
  "InvoiceClosingDay",
  "InvoiceDueDay"
FROM
  "CreditCard"
WHERE
  "Id" = @Id
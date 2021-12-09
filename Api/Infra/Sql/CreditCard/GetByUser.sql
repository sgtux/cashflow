SELECT
  "Id",
  "Name",
  "UserId",
  "InvoiceDay"
FROM
  "CreditCard"
WHERE
  "UserId" = @UserId
ORDER BY
  "Name"
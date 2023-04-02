SELECT
  p.Id,
  p.Description,
  p.UserId,
  p.Type,
  p.CreditCardId,
  i.Id AS Installments_Id,
  i.PaymentId AS Installments_PaymentId,
  i.Value AS Installments_Value,
  i.PaidValue AS Installments_PaidValue,
  i.Number AS Installments_Number,
  i.Date AS Installments_Date,
  i.PaidDate AS Installments_PaidDate,
  i.Exempt AS Installments_Exempt,
  c.Id AS CreditCard_Id,
  c.Name AS CreditCard_Name,
  c.InvoiceClosingDay AS CreditCard_InvoiceClosingDay,
  c.InvoiceDueDay AS CreditCard_InvoiceDueDay
FROM
  Payment p
  JOIN Installment i ON p.Id = i.PaymentId
  LEFT JOIN CreditCard c ON c.Id = p.CreditCardId
WHERE
  p.Id = @Id
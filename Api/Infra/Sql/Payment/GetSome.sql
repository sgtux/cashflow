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
  p.UserId = @UserId
  AND (
    @Description IS NULL
    OR LOWER(p.Description) LIKE LOWER(@Description)
  )
  AND (
    @CreditCardIdsStr IS NULL
    OR p.CreditCardId IN @CreditCardIds
  )
  AND (
    @StartDate IS NULL
    OR EXISTS (
      SELECT
        NULL
      FROM
        Installment
      WHERE
        PaymentId = p.Id
        AND Date >= @StartDate
    )
  )
  AND (
    @EndDate IS NULL
    OR EXISTS (
      SELECT
        NULL
      FROM
        Installment
      WHERE
        PaymentId = p.Id
        AND Date <= @EndDate
    )
  )
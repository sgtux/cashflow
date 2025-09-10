UPDATE
  Installment
SET
  Value = @Value,
  Number = @Number,
  Date = @Date,
  PaidDate = @PaidDate,
  PaidValue = @PaidValue,
  Exempt = @Exempt
WHERE
  Id = @Id
  AND PaymentId = @PaymentId
INSERT INTO
  Installment (
    PaymentId,
    Value,
    Number,
    Date,
    PaidDate,
    PaidValue,
    Exempt
  )
VALUES
  (
    @PaymentId,
    @Value,
    @Number,
    @Date,
    @PaidDate,
    @PaidValue,
    @Exempt
  )
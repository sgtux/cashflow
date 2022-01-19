class PaymentProjectionModel {
  final String description;
  final int number;
  final int condition;
  final String conditionText;
  final int qtdInstallments;
  final num cost;
  final String typeText;
  final String creditCardName;
  final bool isIn;

  PaymentProjectionModel(
      {required this.description,
      required this.number,
      required this.condition,
      required this.conditionText,
      required this.qtdInstallments,
      required this.cost,
      required this.isIn,
      required this.typeText,
      required this.creditCardName});

  factory PaymentProjectionModel.fromMap(Map<String, dynamic> map) {
    return PaymentProjectionModel(
        description: map['description'],
        number: map['number'],
        condition: map['condition'],
        conditionText: map['conditionText'],
        qtdInstallments: map['qtdInstallments'],
        cost: map['cost'],
        typeText: map['typeText'],
        isIn: map['type']['in'],
        creditCardName: map['creditCardName']);
  }
}

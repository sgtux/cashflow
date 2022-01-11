class HouseholdExpense {
  final int id;
  final String description;
  final double value;
  final DateTime date;

  HouseholdExpense(
      {required this.id,
      required this.description,
      required this.value,
      required this.date});

  factory HouseholdExpense.fromMap(Map<String, dynamic> map) {
    return HouseholdExpense(
        id: map['id'],
        description: map['description'],
        value: map['value'],
        date: DateTime.parse(map['date']));
  }
}

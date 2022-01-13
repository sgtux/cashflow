import 'package:cashflow_app/src/models/model_base.dart';

class HouseholdExpenseModel extends ModelBase {
  final int id;
  final String description;
  final double value;
  final DateTime date;

  HouseholdExpenseModel(
      {required this.id,
      required this.description,
      required this.value,
      required this.date});

  factory HouseholdExpenseModel.fromMap(Map<String, dynamic> map) {
    return HouseholdExpenseModel(
        id: map['id'],
        description: map['description'],
        value: map['value'],
        date: DateTime.parse(map['date']));
  }

  @override
  Map<String, dynamic> toMap() {
    return {
      'id': id,
      'description': description,
      'value': value,
      'date': date.toIso8601String()
    };
  }
}

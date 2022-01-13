import 'package:cashflow_app/src/models/model_base.dart';

class FuelExpenseModel extends ModelBase {
  final int id;
  final int miliage;
  final double valueSupplied;
  final double pricePerLiter;
  final DateTime date;
  final int vehicleId;
  late double litersSupplied;

  FuelExpenseModel(
      {required this.id,
      required this.miliage,
      required this.valueSupplied,
      required this.pricePerLiter,
      required this.date,
      required this.vehicleId,
      litersSupplied});

  factory FuelExpenseModel.fromMap(Map<String, dynamic> map) {
    return FuelExpenseModel(
      id: map['id'],
      miliage: map['description'],
      valueSupplied: double.parse(map['value']),
      pricePerLiter: double.parse(map['value']),
      date: DateTime.parse(map['date']),
      vehicleId: map['value'],
      litersSupplied: double.parse(map['value']),
    );
  }

  @override
  Map<String, dynamic> toMap() {
    return {
      "id": id,
      "miliage": miliage,
      "valueSupplied": valueSupplied,
      "pricePerLiter": pricePerLiter,
      "date": date.toIso8601String(),
      "vehicleId": vehicleId,
      "litersSupplied": litersSupplied,
    };
  }
}

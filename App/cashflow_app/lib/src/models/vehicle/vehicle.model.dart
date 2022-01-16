import 'package:cashflow_app/src/models/model_base.dart';
import 'package:cashflow_app/src/models/vehicle/fuel_expense.model.dart';

class VehicleModel extends ModelBase {
  final int id;
  final String description;
  final num miliageTraveled;
  final num miliagePerLiter;
  final List<FuelExpenseModel> fuelExpenses;

  VehicleModel(
      {required this.id,
      required this.description,
      required this.miliageTraveled,
      required this.miliagePerLiter,
      required this.fuelExpenses});

  factory VehicleModel.fromMap(Map<String, dynamic> map) => VehicleModel(
      id: map['id'],
      description: map['description'],
      miliagePerLiter: map['miliagePerLiter'],
      miliageTraveled: map['miliageTraveled'],
      fuelExpenses: []);

  @override
  Map<String, dynamic> toMap() {
    return {"id": id, "description": description};
  }
}

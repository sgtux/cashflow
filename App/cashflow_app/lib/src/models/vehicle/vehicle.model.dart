import 'package:cashflow_app/src/models/model_base.dart';
import 'package:cashflow_app/src/models/vehicle/fuel_expense.model.dart';

class VehicleModel extends ModelBase {
  final int id;
  final String description;
  late double miliageTraveled;
  late double miliagePerLiter;
  late List<FuelExpenseModel> fuelExpenses;

  VehicleModel(
      {required this.id,
      required this.description,
      miliageTraveled,
      miliagePerLiter,
      fuelExpenses});

  factory VehicleModel.fromMap(Map<String, dynamic> map) => VehicleModel(
      id: map['id'],
      description: map['description'],
      miliagePerLiter: map['milagePerLiter'],
      miliageTraveled: map['miliageTraveled'],
      fuelExpenses: []);

  @override
  Map<String, dynamic> toMap() {
    return {"id": id, "description": description};
  }
}

class ProjectionModel {
  final String monthYear;
  final num totalIn;
  final num totalOut;
  final num total;
  final num accumulatedCost;

  ProjectionModel(
      {required this.monthYear,
      required this.totalIn,
      required this.totalOut,
      required this.total,
      required this.accumulatedCost});

  factory ProjectionModel.fromMap(String yearMonth, Map<String, dynamic> map) {
    return ProjectionModel(
        monthYear: yearMonth,
        accumulatedCost: map['accumulatedCost'],
        total: map['total'],
        totalIn: map['totalIn'],
        totalOut: map['totalOut']);
  }
}

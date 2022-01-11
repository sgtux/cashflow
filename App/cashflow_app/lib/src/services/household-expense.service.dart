import 'package:cashflow_app/src/models/household-expense/HouseholdExpense.dart';
import 'package:flutter/material.dart';

import 'http.service.dart';

class HouseholdExpenseService extends HttpService {
  HouseholdExpenseService(BuildContext context) : super(context: context);

  Future<List<HouseholdExpense>> getAll() async {
    final result = await get('HouseholdExpense?month=12&year=2021', null);
    List<HouseholdExpense> list = [];
    if (result.errors.isEmpty && result.data.isNotEmpty) {
      for (var e in (result.data as List)) {
        list.add(HouseholdExpense.fromMap(e));
      }
    }
    return list;
  }
}

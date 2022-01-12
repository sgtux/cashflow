import 'package:cashflow_app/src/models/household-expense/household_expense_model.dart';
import 'package:flutter/material.dart';
import 'http.service.dart';

class HouseholdExpenseService extends HttpService {
  HouseholdExpenseService(BuildContext context) : super(context: context);

  Future<List<HouseholdExpenseModel>> getAll(String month, String year) async {
    final result = await get('HouseholdExpense?month=$month&year=$year');
    List<HouseholdExpenseModel> list = [];
    if (result.errors.isEmpty && result.data.isNotEmpty) {
      for (var e in (result.data as List)) {
        list.add(HouseholdExpenseModel.fromMap(e));
      }
    }
    return list;
  }

  Future save(HouseholdExpenseModel householdExpense) async {
    if (householdExpense.id > 0) {
      await put('HouseholdExpense', householdExpense);
    } else {
      await post('HouseholdExpense', householdExpense);
    }
  }

  Future remove(int id) async {
    await delete('HouseholdExpense/$id');
  }
}

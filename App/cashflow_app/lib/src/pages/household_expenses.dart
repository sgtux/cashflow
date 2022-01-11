import 'package:cashflow_app/src/models/household-expense/HouseholdExpense.dart';
import 'package:cashflow_app/src/services/household-expense.service.dart';
import 'package:cashflow_app/src/utils/string_extensions.dart';
import 'package:flutter/material.dart';

typedef RemoveCallback = void Function(int id);

Container householdExpenseList(List<HouseholdExpense> list,
    RemoveCallback callback, BuildContext context) {
  if (list.isEmpty) {
    return Container(child: const Text("Você não possui Despesas Domésticas."));
  }
  List<Card> items = [];

  for (var e in list) {
    items.add(Card(
        child: ListTile(
      onTap: () {
        // Navigator.pushNamed(context, '/details', arguments: e);
      },
      title: Text(e.description),
      subtitle: Text("${e.value}"),
    )));
  }

  return Container(
      color: Colors.red.shade200,
      child: ListView(
        padding: const EdgeInsets.all(8),
        children: items,
      ));
}

class HouseholdExpenses extends StatefulWidget {
  const HouseholdExpenses({Key? key}) : super(key: key);

  @override
  _HouseholdExpensesState createState() => _HouseholdExpensesState();
}

class _HouseholdExpensesState extends State<HouseholdExpenses> {
  late HouseholdExpenseService householdExpenseService;
  late List<HouseholdExpense> list = [];

  @override
  Widget build(BuildContext context) {
    householdExpenseService = HouseholdExpenseService(context);

    return Scaffold(
      appBar: AppBar(title: const Text("Despesas Domésticas")),
      body: Column(children: [
        ElevatedButton(
            onPressed: () {
              householdExpenseService.getAll().then((value) => {
                    setState(() => {list = value})
                  });
            },
            child: const Text('Atualizar')),
        Expanded(
            child: ListView.builder(
                itemCount: list.length,
                itemBuilder: (BuildContext ctx, int idx) {
                  return Card(
                      child: ListTile(
                    onTap: () {
                      // Navigator.pushNamed(context, '/details', arguments: e);
                    },
                    title: Text(list[idx].description),
                    subtitle: Text(
                        "${toReal(value: list[idx].value)} - ${toDateString(value: list[idx].date)}"),
                  ));
                }))
      ]),
    );
  }
}

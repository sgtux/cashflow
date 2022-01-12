import 'package:cashflow_app/src/pages/household_expense_detail.dart';
import 'package:cashflow_app/src/services/account.service.dart';
import 'package:flutter/material.dart';
import 'src/pages/home.dart';
import 'src/pages/login.dart';
import 'src/pages/household_expenses.dart';
import 'src/services/storage.service.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final accountService = AccountService(context);

    return FutureBuilder(
        future: accountService.validateToken(),
        builder: (BuildContext context, snapshot) {
          if (snapshot.data == true) {
            final token = accountService.storage.getToken();
            final initialRoute =
                token != null && token != '' ? '/home' : '/login';

            return MaterialApp(
              title: 'Cashflow App',
              theme: ThemeData(primarySwatch: Colors.green),
              initialRoute: initialRoute,
              routes: {
                '/home': (context) => const Home(),
                '/login': (context) => const Login(),
                '/household-expenses': (context) => const HouseholdExpenses(),
                '/household-expense-detail': (context) =>
                    const HouseholdExpenseDetail()
              },
            );
          }
          return const CircularProgressIndicator();
        });
  }
}

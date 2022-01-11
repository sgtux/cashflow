import 'package:flutter/material.dart';
import 'src/pages/home.dart';
import 'src/pages/login.dart';
import 'src/pages/household_expenses.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Cashflow App',
      theme: ThemeData(primarySwatch: Colors.green),
      initialRoute: '/login',
      routes: {
        '/home': (context) => const Home(),
        '/login': (context) => const Login(),
        '/household-expenses': (context) => const HouseholdExpenses()
      },
    );
  }
}

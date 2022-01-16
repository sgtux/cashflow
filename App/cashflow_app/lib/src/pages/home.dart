import 'package:cashflow_app/src/pages/household_expense_list.dart';
import 'package:cashflow_app/src/pages/resume_screen.dart';
import 'package:cashflow_app/src/pages/vehicle/vehicle_list.screen.dart';
import "package:flutter/material.dart";

class Home extends StatefulWidget {
  const Home({Key? key}) : super(key: key);

  @override
  _HomeState createState() => _HomeState();
}

class _HomeState extends State<Home> {
  int _index = 0;

  final List<Widget> _screens = const [
    ResumeScreen(),
    VehicleListScreen(),
    HouseholdExpenseList()
  ];

  _incrementTap(index) {
    setState(() {
      _index = index;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text("CASHFLOW")),
      body: _screens[_index],
      bottomNavigationBar: BottomNavigationBar(
        selectedIconTheme: const IconThemeData(size: 40),
        unselectedLabelStyle: const TextStyle(fontSize: 12),
        selectedLabelStyle: const TextStyle(fontSize: 10),
        unselectedIconTheme: const IconThemeData(color: Colors.grey),
        currentIndex: _index,
        elevation: 5,
        onTap: (idx) {
          _incrementTap(idx);
        },
        items: const [
          BottomNavigationBarItem(icon: Icon(Icons.home), label: 'Home'),
          BottomNavigationBarItem(
              icon: Icon(Icons.motorcycle), label: 'Veículos'),
          BottomNavigationBarItem(
              icon: Icon(Icons.bar_chart), label: 'Despesas'),
        ],
      ),
    );
  }
}

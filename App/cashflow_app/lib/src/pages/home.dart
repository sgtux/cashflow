import "package:flutter/material.dart";

class Home extends StatelessWidget {
  const Home({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text("Cashflow")),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            ElevatedButton(
                onPressed: () {
                  Navigator.pushNamed(context, '/household-expenses');
                },
                child: const Text("Despesas Domésticas")),
            ElevatedButton(
                onPressed: () {
                  Navigator.pushNamed(context, '/vehicles');
                },
                child: const Text("Veículos"))
          ],
        ),
      ),
    );
  }
}

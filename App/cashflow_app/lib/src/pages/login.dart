import 'package:cashflow_app/src/utils/constants.dart';
import 'package:flutter/material.dart';
import '../services/account.service.dart';
import '../services/storage.service.dart';

class Login extends StatefulWidget {
  const Login({Key? key}) : super(key: key);

  @override
  _LoginState createState() => _LoginState();
}

class _LoginState extends State<Login> {
  final StorageService storageService = StorageService();

  late AccountService accountService;
  late String name = '';
  late String password = '';
  late bool isLoading = false;

  @override
  Widget build(BuildContext context) {
    accountService = AccountService(context);

    return Scaffold(
        appBar: AppBar(
          title: const Text("CASHFLOW"),
        ),
        body: Center(
            child: Container(
          width: 300,
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              TextField(
                onChanged: (text) {
                  setState(() {
                    name = text;
                  });
                },
                decoration: const InputDecoration(
                    icon: Icon(Icons.person), hintText: 'Nome'),
              ),
              TextField(
                onChanged: (text) {
                  setState(() {
                    password = text;
                  });
                },
                autocorrect: false,
                obscureText: true,
                enableSuggestions: false,
                decoration: const InputDecoration(
                    icon: Icon(Icons.lock), hintText: 'Senha'),
              ),
              const SizedBox(
                height: 20,
                width: 20,
              ),
              isLoading
                  ? const CircularProgressIndicator()
                  : ElevatedButton(
                      onPressed: () {
                        setState(() {
                          isLoading = true;
                        });
                        accountService.login(name, password).then((res) {
                          if (res != null) {
                            storageService.setToken(res.token);
                            Navigator.pushNamedAndRemoveUntil(
                                context, Routes.home, (_) => false);
                          }
                        }).whenComplete(
                            () => setState(() => {isLoading = false}));
                      },
                      child: const Text("Entrar")),
            ],
          ),
        )));
  }
}

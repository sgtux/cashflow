import 'package:flutter/material.dart';
import '../services/account.service.dart';
import '../services/storage.service.dart';

class Login extends StatefulWidget {
  const Login({Key? key}) : super(key: key);

  @override
  _LoginState createState() => _LoginState();
}

class _LoginState extends State<Login> {
  final StorageService storage = StorageService();

  late AccountService accountService;
  late String name = '';
  late String password = '';
  late bool isLoading = false;

  @override
  void initState() {
    final token = storage.getToken();
    if (token != null && token != '') {
      Navigator.pushNamed(context, '/home');
    }
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    accountService = AccountService(context);

    return Scaffold(
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
          isLoading
              ? const CircularProgressIndicator()
              : ElevatedButton(
                  onPressed: () {
                    setState(() {
                      isLoading = true;
                    });
                    accountService.login(name, password).then((res) {
                      if (res != null) {
                        storage.setToken(res.token);
                        Navigator.pushNamed(context, '/home');
                      }
                    }).whenComplete(() => setState(() => {isLoading = false}));
                  },
                  child: const Text("Entrar"))
        ],
      ),
    )));
  }
}

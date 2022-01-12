import 'package:cashflow_app/src/models/model_base.dart';

class LoginModel extends ModelBase {
  final String name;
  final String password;

  LoginModel({required this.name, required this.password});

  @override
  Map<String, dynamic> toMap() {
    return {'nickName': name, 'password': password};
  }
}

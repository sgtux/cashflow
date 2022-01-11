import 'package:cashflow_app/src/models/account/login_model_result.dart';
import 'package:flutter/material.dart';

import 'http.service.dart';

class AccountService extends HttpService {
  AccountService(BuildContext context) : super(context: context);

  Future<LoginModelResult?> login(String name, String password) async {
    final result =
        await post('token', {'NickName': name, 'password': password});
    final hasKeys = result.data.isNotEmpty;
    return hasKeys ? LoginModelResult.fromMap(result.data) : null;
  }
}

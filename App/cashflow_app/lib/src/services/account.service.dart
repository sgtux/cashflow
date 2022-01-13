import 'package:cashflow_app/src/models/account/login_model.dart';
import 'package:cashflow_app/src/models/account/login_model_result.dart';
import 'package:cashflow_app/src/services/storage.service.dart';
import 'package:flutter/material.dart';

import 'http.service.dart';

class AccountService extends HttpService {
  AccountService(BuildContext context) : super(context: context);

  Future<LoginModelResult?> login(String name, String password) async {
    final result =
        await post('token', LoginModel(name: name, password: password));

    if (result.isValid()) return LoginModelResult.fromMap(result.data);

    return null;
  }

  Future<bool> validateToken() async {
    final storageService = StorageService();
    await storageService.storage.ready;
    try {
      await get('account');
    } catch (err) {
      storage.setToken(null);
    }
    return true;
  }
}
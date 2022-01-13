import 'package:cashflow_app/src/services/storage.service.dart';
import 'package:flutter/cupertino.dart';

import 'constants.dart';

void handleHttpException(dynamic error, BuildContext context) {
  if (error is Map) {
    if (error.containsKey('code') && error['code'] == 401) {
      final storage = StorageService();
      storage.setToken(null);
      Navigator.pushNamedAndRemoveUntil(context, Routes.login, (_) => false);
    }
  }
}

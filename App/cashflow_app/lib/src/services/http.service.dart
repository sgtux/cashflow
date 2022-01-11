import 'dart:convert';
import 'package:cashflow_app/src/services/storage.service.dart';
import 'package:cashflow_app/src/utils/constants.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import '../models/result_model.dart';

class HttpService {
  final BuildContext context;
  final StorageService storage = StorageService();

  HttpService({required this.context});

  Future<ResultModel> post(String url, Object body) async {
    final response = await http.post(Uri.parse("$urlApi/$url"),
        headers: {"Content-Type": "application/json"}, body: json.encode(body));

    final resultModel = ResultModel.fromJson(json.decode(response.body));

    if (response.statusCode != 200) {
      for (var error in resultModel.errors) {
        ScaffoldMessenger.of(context).showSnackBar(SnackBar(
          content: Text(error),
          backgroundColor: Colors.red,
        ));
      }
    }
    return resultModel;
  }

  Future<ResultModel> get(String url, Map<String, String>? params) async {
    final token = storage.getToken();
    final response = await http.get(Uri.parse("$urlApi/$url"), headers: {
      "Content-Type": "application/json",
      'Authorization': 'Bearer $token'
    });

    final resultModel = ResultModel.fromJson(json.decode(response.body));

    if (response.statusCode != 200) {
      for (var error in resultModel.errors) {
        ScaffoldMessenger.of(context).showSnackBar(SnackBar(
          content: Text(error),
          backgroundColor: Colors.red,
        ));
      }
    }
    return resultModel;
  }
}

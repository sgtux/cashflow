import 'package:cashflow_app/src/models/home/projection-model.dart';
import 'package:flutter/material.dart';
import 'http.service.dart';

class HomeService extends HttpService {
  HomeService(BuildContext context) : super(context: context);

  Future<List<ProjectionModel>> getProjection() async {
    final result = await get('Home/Projection?month=12&year=2022');
    List<ProjectionModel> list = [];
    if (result.errors.isEmpty && result.data.isNotEmpty) {
      Map map = result.data as Map<String, dynamic>;
      for (var key in map.keys) {
        list.add(ProjectionModel.fromMap(key, map[key]));
      }
    }
    return list;
  }
}

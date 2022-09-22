import 'package:cashflow_app/src/models/home/home_data_model.dart';
import 'package:cashflow_app/src/models/home/projection_model.dart';
import 'package:flutter/material.dart';
import 'http.service.dart';

class HomeService extends HttpService {
  HomeService(BuildContext context) : super(context: context);

  Future<List<ProjectionModel>> getProjection() async {
    var date = DateTime.now().add(const Duration(days: 366));
    final result = await get('Projection?month=${date.month}&year=${date.year}');
    List<ProjectionModel> list = [];
    if (result.errors.isEmpty && result.data.isNotEmpty) {
      Map map = result.data as Map<String, dynamic>;
      for (var key in map.keys) {
        list.add(ProjectionModel.fromMap(key, map[key]));
      }
    }
    return list;
  }

  Future<List<HomeDataModel>> getHomeData() async {
    final now = DateTime.now();
    final result = await get('Home?month=${now.month}&year=${now.year}');
    List<HomeDataModel> list = [];
    if (result.errors.isEmpty && result.data.isNotEmpty) {
      for (var item in (result.data as List)) {
        list.add(HomeDataModel.fromMap(item));
      }
    }
    return list;
  }
}

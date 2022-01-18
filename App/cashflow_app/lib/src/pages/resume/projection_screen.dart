import 'package:cashflow_app/src/models/home/projection_model.dart';
import 'package:cashflow_app/src/services/home.service.dart';
import 'package:cashflow_app/src/utils/exception_handler.dart';
import 'package:cashflow_app/src/utils/string_extensions.dart';
import 'package:flutter/material.dart';

class ProjectionScreen extends StatefulWidget {
  const ProjectionScreen({Key? key}) : super(key: key);

  @override
  State<StatefulWidget> createState() => _ProjectionScreenState();
}

class _ProjectionScreenState extends State<ProjectionScreen> {
  late HomeService homeService;
  bool isLoading = false;
  List<ProjectionModel> list = [];

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance?.addPostFrameCallback((_) => refresh());
  }

  void refresh() {
    setState(() {
      isLoading = true;
    });
    homeService.getProjection().then((value) {
      setState(() {
        isLoading = false;
        list = value;
      });
    }).catchError((error) {
      setState(() {
        isLoading = false;
      });
      handleHttpException(error, context);
    });
  }

  @override
  Widget build(BuildContext context) {
    homeService = HomeService(context);

    return Scaffold(
        body: isLoading
            ? const Center(child: CircularProgressIndicator())
            : Column(children: [
                const SizedBox(
                  height: 10,
                ),
                Expanded(
                    child: ListView.builder(
                        itemCount: list.length,
                        itemBuilder: (BuildContext ctx, int idx) {
                          return Card(
                              child: ListTile(
                            title: Text(toMonthYearText(list[idx].monthYear)),
                            subtitle: Row(children: [
                              Text(
                                toReal(
                                    value:
                                        list[idx].accumulatedCost.toDouble()),
                                style: TextStyle(
                                    color: list[idx].accumulatedCost > 0
                                        ? Colors.green.shade300
                                        : Colors.red.shade300,
                                    fontWeight: FontWeight.bold),
                              ),
                            ]),
                          ));
                        }))
              ]));
  }
}

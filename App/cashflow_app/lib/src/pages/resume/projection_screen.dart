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
  String selectedMonthYear = '';

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

    return SingleChildScrollView(
        child: isLoading
            ? const Center(child: CircularProgressIndicator())
            : Column(children: [
                const SizedBox(
                  height: 10,
                ),
                ExpansionPanelList(
                  expansionCallback: (int index, bool isExpanded) {
                    setState(() {
                      selectedMonthYear =
                          isExpanded ? '' : list[index].monthYear;
                    });
                  },
                  children: list.map<ExpansionPanel>((ProjectionModel item) {
                    return ExpansionPanel(
                      headerBuilder: (BuildContext context, bool isExpanded) {
                        return ListTile(
                          title: Row(children: [
                            Text("${toMonthYearText(item.monthYear)} - ",
                                style: TextStyle(color: Colors.grey.shade600)),
                            Text(
                              toReal(value: item.accumulatedValue.toDouble()),
                              style: TextStyle(
                                  color: item.accumulatedValue > 0
                                      ? Colors.green.shade300
                                      : Colors.red.shade300,
                                  fontWeight: FontWeight.bold),
                            ),
                          ]),
                        );
                      },
                      body: Container(
                        padding: const EdgeInsets.all(10.0),
                        child: Column(
                          children: [
                            Column(
                                children: item.payments.map((p) {
                              return Container(
                                  padding: const EdgeInsets.all(2.0),
                                  color: item.payments.indexOf(p) % 2 == 0
                                      ? Colors.grey.shade200
                                      : Colors.grey.shade300,
                                  child: Row(
                                      mainAxisAlignment:
                                          MainAxisAlignment.spaceBetween,
                                      children: [
                                        Text(
                                          elipsis(p.description, 30),
                                          style: TextStyle(
                                              fontSize: 12,
                                              color: Colors.grey.shade600),
                                        ),
                                        p.qtdInstallments > 0
                                            ? Text(
                                                "${p.number}/${p.qtdInstallments}",
                                                style: TextStyle(
                                                    fontSize: 10,
                                                    color:
                                                        Colors.grey.shade600),
                                              )
                                            : const SizedBox(),
                                        Text(
                                          toReal(value: p.value.toDouble()),
                                          style: TextStyle(
                                              fontSize: 12,
                                              color: p.isIn
                                                  ? Colors.green.shade300
                                                  : Colors.red.shade300),
                                        )
                                      ]));
                            }).toList()),
                            const SizedBox(height: 10),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.end,
                              children: [
                                Text("Entrada: ",
                                    style:
                                        TextStyle(color: Colors.grey.shade600)),
                                Text(
                                  toReal(value: item.totalIn.toDouble()),
                                  style: TextStyle(
                                      color: Colors.green.shade300,
                                      fontWeight: FontWeight.bold),
                                )
                              ],
                            ),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.end,
                              children: [
                                Text("Saída: ",
                                    style:
                                        TextStyle(color: Colors.grey.shade600)),
                                Text(
                                  toReal(value: item.totalOut.toDouble()),
                                  style: TextStyle(
                                      color: Colors.red.shade300,
                                      fontWeight: FontWeight.bold),
                                )
                              ],
                            ),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.end,
                              children: [
                                Text("Líquido: ",
                                    style:
                                        TextStyle(color: Colors.grey.shade600)),
                                Text(
                                  toReal(value: item.total.toDouble()),
                                  style: TextStyle(
                                      color: item.total > 0
                                          ? Colors.green.shade300
                                          : Colors.red.shade300,
                                      fontWeight: FontWeight.bold),
                                )
                              ],
                            )
                          ],
                        ),
                      ),
                      isExpanded: selectedMonthYear == item.monthYear,
                    );
                  }).toList(),
                )
              ]));
  }
}

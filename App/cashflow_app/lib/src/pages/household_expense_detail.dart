import 'package:cashflow_app/src/utils/exception_handler.dart';
import 'package:intl/intl.dart';
import 'package:cashflow_app/src/utils/string_extensions.dart';
import 'package:cashflow_app/src/models/household-expense/household_expense_model.dart';
import 'package:cashflow_app/src/services/household-expense.service.dart';
import 'package:flutter/material.dart';
// ignore: import_of_legacy_library_into_null_safe
import 'package:flutter_masked_text/flutter_masked_text.dart';

typedef RemoveCallback = void Function(int id);

class HouseholdExpenseDetail extends StatefulWidget {
  const HouseholdExpenseDetail({Key? key}) : super(key: key);

  @override
  _HouseholdExpenseDetailState createState() => _HouseholdExpenseDetailState();
}

class _HouseholdExpenseDetailState extends State<HouseholdExpenseDetail> {
  late HouseholdExpenseService householdExpenseService;
  late bool isLoading = false;

  final _formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    householdExpenseService = HouseholdExpenseService(context);

    final valueController = MoneyMaskedTextController(
        decimalSeparator: ',', thousandSeparator: '.', leftSymbol: 'R\$ ');

    final dateController = TextEditingController();
    final descriptionController = TextEditingController();

    final householdExpense = (ModalRoute.of(context)?.settings.arguments
            as HouseholdExpenseModel?) ??
        HouseholdExpenseModel(
            id: 0, description: '', date: DateTime.now(), value: 0);

    descriptionController.text = householdExpense.description;
    valueController.updateValue(householdExpense.value);
    dateController.text =
        toDateString(value: householdExpense.date, separator: '/');

    return Scaffold(
        appBar: AppBar(title: const Text("Despesa Doméstica")),
        body: SingleChildScrollView(
          child: Card(
              child: Form(
                  key: _formKey,
                  child: Container(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 20.0, vertical: 20.0),
                      // child: Expanded(
                      child: Column(
                        children: [
                          TextFormField(
                            controller: descriptionController,
                            decoration:
                                const InputDecoration(labelText: 'Descrição'),
                            validator: (value) {
                              if (value == null || value.isEmpty) {
                                return 'Campo obrigatório.';
                              }
                              return null;
                            },
                          ),
                          TextFormField(
                            controller: valueController,
                            keyboardType: TextInputType.number,
                            decoration:
                                const InputDecoration(labelText: 'Valor'),
                            validator: (value) {
                              if (valueController.numberValue <= 0) {
                                return 'Campo obrigatório.';
                              }
                              return null;
                            },
                          ),
                          TextFormField(
                            showCursor: true,
                            readOnly: true,
                            decoration:
                                const InputDecoration(labelText: 'Data'),
                            controller: dateController,
                            validator: (value) {
                              if (value == null || value.isEmpty) {
                                return 'Campo obrigatório.';
                              }
                              return null;
                            },
                            onTap: () {
                              showDatePicker(
                                      context: context,
                                      initialDate: DateTime.now(),
                                      firstDate: DateTime(2018),
                                      lastDate: DateTime(2026))
                                  .then((date) => {
                                        dateController.text = toDateString(
                                            value: date, separator: '/')
                                      });
                            },
                          ),
                          const SizedBox(
                            height: 20,
                          ),
                          isLoading
                              ? const CircularProgressIndicator()
                              : Row(
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  children: [
                                    OutlinedButton(
                                      onPressed: () {
                                        Navigator.pop(context);
                                      },
                                      child: const Text('VOLTAR'),
                                    ),
                                    const SizedBox(
                                      width: 10,
                                    ),
                                    ElevatedButton(
                                      onPressed: () {
                                        if (_formKey.currentState!.validate()) {
                                          setState(() {
                                            isLoading = true;
                                          });
                                          final model = HouseholdExpenseModel(
                                              id: householdExpense.id,
                                              description:
                                                  descriptionController.text,
                                              value:
                                                  valueController.numberValue,
                                              date: DateFormat('dd/MM/yyyy')
                                                  .parse(dateController.text));
                                          householdExpenseService
                                              .save(model)
                                              .then((value) {
                                            setState(() => {isLoading = false});
                                            Navigator.of(context).pop();
                                          }).catchError((error) {
                                            setState(() {
                                              isLoading = false;
                                            });
                                            handleHttpException(error, context);
                                          });
                                        }
                                      },
                                      child: const Text('SALVAR'),
                                    )
                                  ],
                                ),
                        ],
                      ))
                  // ),
                  )),
        ));
  }
}

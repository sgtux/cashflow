import 'package:cashflow_app/src/models/vehicle/fuel_expense.model.dart';
import 'package:cashflow_app/src/services/fuel_expense.service.dart';
import 'package:cashflow_app/src/utils/exception_handler.dart';
import 'package:cashflow_app/src/utils/string_extensions.dart';
import 'package:flutter/material.dart';
import 'package:flutter_masked_text/flutter_masked_text.dart';
import 'package:intl/intl.dart';

class FuelExpenseDetailScreen extends StatefulWidget {
  const FuelExpenseDetailScreen({Key? key}) : super(key: key);

  @override
  _FuelExpenseDetailScreenState createState() =>
      _FuelExpenseDetailScreenState();
}

class _FuelExpenseDetailScreenState extends State<FuelExpenseDetailScreen> {
  late FuelExpenseService _fuelExpenseService;
  late bool isLoading = false;
  FuelExpenseModel? fuelExpense;

  final _formKey = GlobalKey<FormState>();
  final miliageController = TextEditingController();
  final valueSuppliedController = MoneyMaskedTextController(
      decimalSeparator: ',', thousandSeparator: '.', leftSymbol: 'R\$ ');
  final pricePerLiterController = MoneyMaskedTextController(
      decimalSeparator: ',', thousandSeparator: '.', leftSymbol: 'R\$ ');
  final dateController = TextEditingController();

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance?.addPostFrameCallback((_) => refresh());
  }

  void refresh() {
    FuelExpenseModel fuelExpenseTemp =
        (ModalRoute.of(context)!.settings.arguments as FuelExpenseModel);

    miliageController.text = fuelExpenseTemp.miliage.toString();
    valueSuppliedController
        .updateValue(fuelExpenseTemp.valueSupplied.toDouble());
    pricePerLiterController
        .updateValue(fuelExpenseTemp.pricePerLiter.toDouble());
    dateController.text =
        toDateString(value: fuelExpenseTemp.date, separator: '/');

    setState(() {
      fuelExpense = fuelExpenseTemp;
    });
  }

  @override
  Widget build(BuildContext context) {
    _fuelExpenseService = FuelExpenseService(context);

    return Scaffold(
        appBar: AppBar(title: const Text("Veículo")),
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
                            keyboardType: TextInputType.number,
                            controller: miliageController,
                            decoration: const InputDecoration(
                                labelText: 'Quilometragem'),
                            validator: (value) {
                              if (value == null ||
                                  value.isEmpty ||
                                  double.parse(value) <= 0) {
                                return 'Campo obrigatório.';
                              }
                              return null;
                            },
                          ),
                          TextFormField(
                            keyboardType: TextInputType.number,
                            controller: pricePerLiterController,
                            decoration: const InputDecoration(
                                labelText: 'Valor por litro'),
                            validator: (value) {
                              if (pricePerLiterController.numberValue <= 0) {
                                return 'Campo obrigatório.';
                              }
                              return null;
                            },
                          ),
                          TextFormField(
                            keyboardType: TextInputType.number,
                            controller: valueSuppliedController,
                            decoration: const InputDecoration(
                                labelText: 'Valor abastecido'),
                            validator: (value) {
                              if (valueSuppliedController.numberValue <= 0) {
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
                                          final model = FuelExpenseModel(
                                            id: fuelExpense!.id,
                                            vehicleId: fuelExpense!.vehicleId,
                                            miliage: int.parse(
                                                miliageController.text),
                                            pricePerLiter:
                                                pricePerLiterController
                                                    .numberValue,
                                            valueSupplied:
                                                valueSuppliedController
                                                    .numberValue,
                                            date: DateFormat('dd/MM/yyyy')
                                                .parse(dateController.text),
                                          );
                                          _fuelExpenseService
                                              .save(model)
                                              .then((value) {
                                            setState(() => {isLoading = false});
                                            if (value.isValid()) {
                                              Navigator.of(context).pop();
                                            }
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

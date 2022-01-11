import 'package:flutter_masked_text/flutter_masked_text.dart';

String toReal({required double value}) {
  final controller =
      MoneyMaskedTextController(decimalSeparator: '.', thousandSeparator: ',');
  controller.updateValue(value);
  return "R\$${controller.text}";
}

String toDateString({required DateTime value}) {
  String day = "${value.day}".padLeft(2, '0');
  String month = "${value.day}".padLeft(2, '0');
  return "$day-$month-${value.year}";
}

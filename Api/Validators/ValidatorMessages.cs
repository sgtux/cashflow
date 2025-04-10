namespace Cashflow.Api.Validators
{
    public static class ValidatorMessages
    {
        public static string FieldIsRequired(string fieldName) => string.Format("O campo '{0}' é obrigatório.", fieldName);

        public static string FieldMinLength(string fieldName, int length) => string.Format("O campo '{0}' deve ter pelo menos {1} caracteres.", fieldName, length);

        public static string FieldMaxLength(string fieldName, int length) => string.Format("O campo '{0}' deve ter no máximo {1} caracteres.", fieldName, length);

        public static string MinValue(string fieldName, int value) => string.Format("O valor mínimo para o campo '{0}' é {1}.", fieldName, value);

        public static string MaxValue(string fieldName, int value) => string.Format("O valor máximo para o campo '{0}' é {1}.", fieldName, value);

        public static string GreaterThan(string fieldName, int value) => string.Format("O campo '{0}' deve ser maior que {1}.", fieldName, value);

        public static string BetweenValue(string fieldName, int start, int end) => string.Format("O valor do campo '{0}' deve estar entre {1} e {2}.", fieldName, start, end);

        public static string NotFound(string name) => string.Format("{0} não encontrado(a).", name);

        public static UserMessages User => new UserMessages();

        public static PaymentMessages Payment => new PaymentMessages();

        public static CreditCardMessages CreditCard => new CreditCardMessages();

        public static SalaryMessages Salary => new SalaryMessages();

        public static VehicleMessages Vehicle => new VehicleMessages();

        public static RecurringExpenseMessages RecurringExpense => new RecurringExpenseMessages();

        public class UserMessages
        {
            public string EmailAlreadyInUse = "O Email informado já está sendo utilizado.";

            public string EmailPattern = "Deve ser informado um Email válido.";

            public string LoginFailed = "Usuário ou senha inválidos.";

            public string MaximumSystemUsers = "Número máximo de usuários atingido.";
        }

        public class PaymentMessages
        {
            public string InstallmentsRequired = "O pagamento deve ter pelo menos 1 parcela.";

            public string PaymentTypeInvalid = "O tipo do pagamento é inválido.";

            public string PaymentCoditionInvalid = "A condição do pagamento é inválida.";

            public string InstallmentWithInvalidValue = "Parcela com valor inválido.";

            public string InstallmentWithInvalidPaidValue = "Parcela com valor pago inválido.";

            public string ExemptInstallmentWithValue = "Parcela isenta com valor pago informado.";

            public string InstallmentWithInvalidDate = "Parcela com Data de Vencimento inválida.";

            public string InstallmentWithInvalidPaidDate = "Parcela com Data do Pagamento inválida.";

            public string InstallmentWithMaxLengthExceded = "O número máximo de parcelas permitido é 72.";

            public string InstallmentWithInvalidNumber = "Parcela com número inválido.";

            public string InstallmentWithRepeatedNumbers = "Parcela com número repetido.";
        }

        public class CreditCardMessages
        {
            public string BindedWithPayments = "Este cartão está vinculado à algum Parcelamento e não pode ser removido.";

            public string BindedHouseholdExpense = "Este cartão está vinculado à alguma Despesa e não pode ser removido.";
        }

        public class SalaryMessages
        {
            public string AlreadySalaryForThisMonthYear = "Já existe um salário para este mês/ano.";
        }

        public class VehicleMessages
        {
            public string HasFuelExpenses = "O veículo possui despesas cadastradas e não pode ser removido.";
        }

        public class RecurringExpenseMessages
        {
            public string HasHistory = "A despesa recorrente possui histórico e não pode ser removida.";
        }
    }
}
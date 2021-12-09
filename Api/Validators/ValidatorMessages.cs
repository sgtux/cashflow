namespace Cashflow.Api.Validators
{
    public static class ValidatorMessages
    {
        public static string FieldIsRequired(string fieldName) => string.Format("O campo '{0}' é obrigatório.", fieldName);

        public static string FieldMinLength(string fieldName, int length) => string.Format("O campo '{0}' deve ter pelo menos {1} caracteres.", fieldName, length);

        public static string FieldMaxLength(string fieldName, int length) => string.Format("O campo '{0}' deve ter no máximo {1} caracteres.", fieldName, length);

        public static string MinValue(string fieldName, int value) => string.Format("O valor mínimo para o campo '{0}' é {1}.", fieldName, value);

        public static string MaxValue(string fieldName, int value) => string.Format("O valor máximo para o campo '{0}' é {1}.", fieldName, value);

        public static string BetweenValue(string fieldName, int start, int end) => string.Format("O valor do campo '{0}' deve estar entre {1} e {2}.", fieldName, start, end);

        public static string NotFound(string name) => string.Format("{0} não encontrado.", name);

        public static UserMessages User = new UserMessages();

        public static PaymentMessages Payment = new PaymentMessages();

        public static CreditCardMessages CreditCard = new CreditCardMessages();

        public static SalaryMessages Salary = new SalaryMessages();

        public static DailyExpensesMessages DailyExpenses = new DailyExpensesMessages();

        public static VehicleMessages Vehicle = new VehicleMessages();

        public class UserMessages
        {
            public string NickNameAlreadyInUse = "O Nick Name informado já está sendo utilizado.";

            public string NotFound = "Usuário não encontrado.";

            public string NickNamePattern = "O Nick Name deve conter apenas números, letras ou os símbolos _$#@!&.";

            public string LoginFailed = "Usuário ou senha inválidos.";
        }

        public class PaymentMessages
        {
            public string NotFound = "Pagamento não encontrado.";

            public string DescriptionRequired = "O campo 'Descrição' é obrigatório.";

            public string InstallmentsRequired = "O pagamento deve ter pelo menos 1 parcela.";

            public string PaymentTypeInvalid = "O tipo do pagamento é inválido.";

            public string PaymentCoditionInvalid = "A condição do pagamento é inválida.";

            public string InstallmentWithInvalidValue = "Parcela com Valor inválido.";

            public string InstallmentWithInvalidDate = "Parcela com Data de Vencimento inválida.";

            public string InstallmentWithInvalidPaidDate = "Parcela com Data do Pagamento inválida.";

            public string InstallmentWithMaxLengthExceded = "O número máximo de parcelas permitido é 72.";

            public string InstallmentWithInvalidNumber = "Parcela com número inválido.";

            public string InstallmentWithRepeatednNumbers = "Parcela com número repetido.";
        }

        public class CreditCardMessages
        {
            public string NameRequired = "O campo 'Nome' é obrigatório.";

            public string NotFound = "Cartão de crédito não encontrado.";

            public string UserIdRequired = "O usuário é inválido.";

            public string BindedWithPayments = "Este cartão está vinculado à algum pagamento e não pode ser removido.";
        }

        public class SalaryMessages
        {
            public string InvalidStartDate = "O campo Data Início é inválido.";

            public string InvalidEndDate = "O campo Data Fim é inválido.";

            public string ValueMustBeMoreThenZero = "O campo Valor deve ser maior que zero.";

            public string EndDateMustBeMoreThenStartDate = "A Data Fim deve ser maior que a Data Início.";

            public string AnotherCurrentSalary = "Tem outro salário vigente.";

            public string AnotherSalaryInThisDateRange = "Tem outro salário neste intervalo de datas.";

            public string NotFound = "Salário não encontrado.";
        }

        public class DailyExpensesMessages
        {
            public string InvalidDate = "A data é obrigatória.";

            public string InvalidShopName = "O estabelecimento é inválido.";

            public string InvalidItemsCount = "É necessário informar pelo menos 1 item.";

            public string ItemsWithInvalidPrice = "Há itens com preço inválido.";

            public string ItemsWithInvalidName = "Há itens com o nome inválido.";

            public string ItemsWithInvalidAmount = "O valor 'Quantidade' no item deve estar entre 1 e 1000.";
        }

        public class VehicleMessages
        {
            public string HasFuelExpenses = "O veículo possui despesas cadastradas e não pode ser removido.";
        }
    }
}
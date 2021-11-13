namespace Cashflow.Api.Validators
{
    public static class ValidatorMessages
    {
        public static UserMessages User = new UserMessages();

        public static PaymentMessages Payment = new PaymentMessages();

        public static CreditCardMessages CreditCard = new CreditCardMessages();

        public static SalaryMessages Salary = new SalaryMessages();

        public static DailyExpensesMessages DailyExpenses = new DailyExpensesMessages();

        public class UserMessages
        {
            public string NickNameRequired = "O campo 'Nick Name' deve ter pelo menos 4 caracteres.";

            public string NickNameAlreadyInUse = "O Nick Name informado já está sendo utilizado.";

            public string PasswordRequired = "O campo 'Senha' deve ter pelo menos 8 caracteres.";

            public string NotFound = "Usuário não encontrado.";

            public string NickNamePattern = "O Nick Name deve conter apenas números, letras ou os símbolos _$#@!&.";
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

            public string ItemsWithInvalidAmount = "O valor 'Quantidade' no item deve estar entre 0 e 1000.";
        }
    }
}
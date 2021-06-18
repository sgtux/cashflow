namespace Cashflow.Api.Validators
{
    public static class ValidatorMessages
    {
        public static UserMessages User = new UserMessages();

        public static PaymentMessages Payment = new PaymentMessages();

        public static CreditCardMessages CreditCard = new CreditCardMessages();

        public static SalaryMessages Salary = new SalaryMessages();

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

            public string FixedPaymentWithMoreThenOnePlot = "Pagamento fixo não pode ter mais de uma parcela.";
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
            public string InvalidStartDate = "A Data de Início é inválida.";

            public string InvalidEndDate = "A Data Fim é inválida.";

            public string ValueMustBeMoreThenZero = "O valor deve ser maior que zero";

            public string EndDateMustBeMoreThenStartDate = "A Data Fim deve ser maior que a Data Início.";

            public string AnotherCurrentSalary = "Tem outro salário vigente.";

            public string AnotherSalaryInThisDateRange = "Tem outro salário neste intervalo de datas.";
        }
    }
}
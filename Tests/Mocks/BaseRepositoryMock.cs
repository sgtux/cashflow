using System;
using System.Collections.Generic;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Tests.Mocks
{
    public abstract class BaseRepositoryMock
    {
        private static List<Payment> _payments;

        private static List<CreditCard> _creditCards;

        private static List<User> _users;

        private static List<PaymentType> _paymentTypes;

        public DateTime CurrentDate => new DateTime(2019, 4, 1);

        private List<Payment> CreatePaymentsMock()
        {
            var list = new List<Payment>();
            list.Add(new Payment()
            {
                Id = 1,
                UserId = 1,
                CreditCardId = 1,
                Description = "First Payment",
                FixedPayment = true,
                Type = TypePayment.Expense,
                Installments = new List<Installment>()
                {
                    new Installment() { Id = 1, Cost = 1500, Date = CurrentDate }
                }
            });

            list.Add(new Payment() { Id = 2, UserId = 1, CreditCardId = 1 });
            list.Add(new Payment() { Id = 3, UserId = 1, CreditCardId = 1 });
            list.Add(new Payment() { Id = 4, UserId = 2, CreditCardId = 1 });

            return list;
        }

        private List<CreditCard> CreateCreditCard()
        {
            var list = new List<CreditCard>();
            list.Add(new CreditCard() { Id = 1, UserId = 1, Name = "Primeiro Cartão" });
            list.Add(new CreditCard() { Id = 2, UserId = 1, Name = "Segundo Cartão" });
            return list;
        }

        private List<User> CreateUserMock()
        {
            var users = new List<User>();
            users.Add(new User() { Id = 1, NickName = "Primeiro Usuário" });
            users.Add(new User() { Id = 2, NickName = "Segundo Usuário" });
            return users;
        }

        protected List<Payment> Payments
        {
            get
            {
                if (_payments == null)
                    _payments = CreatePaymentsMock();
                return _payments;
            }
        }

        protected List<CreditCard> CreditCards
        {
            get
            {
                if (_creditCards == null)
                    _creditCards = CreateCreditCard();
                return _creditCards;
            }
        }

        protected List<User> Users
        {
            get
            {
                if (_users == null)
                    _users = CreateUserMock();
                return _users;
            }
        }

        protected List<PaymentType> PaymentTypes
        {
            get
            {
                if (_paymentTypes == null)
                    _paymentTypes = new List<PaymentType>()
                    {
                        new PaymentType(){ Id = 1, Description = "Despesa", In = false },
                        new PaymentType(){ Id = 2, Description = "Renda", In = true },
                        new PaymentType() {Id = 3, Description = "Ganho", In = false },
                        new PaymentType(){ Id = 4, Description = "Crédito", In = true },
                        new PaymentType() { Id = 5, Description = "Lucro", In = true },
                    };
                return _paymentTypes;
            }
        }
    }
}
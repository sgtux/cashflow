using System;
using System.Collections.Generic;
using System.Linq;
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

        private static List<Salary> _salaries;

        private static List<DailyExpenses> _dailyExpenses;

        private static List<Vehicle> _vehicles;

        public DateTime CurrentDate => new DateTime(2019, 4, 1);

        private List<Payment> CreatePaymentsMock()
        {
            return new List<Payment>()
            {
                new Payment()
                {
                    Id = 1,
                    UserId = 1,
                    CreditCardId = 1,
                    Description = "First Payment",
                    Type = PaymentTypes.FirstOrDefault(p => p.Id == (int)PaymentTypeEnum.Expense),
                    Installments = new List<Installment>()
                    {
                        new Installment() { Id = 1, Cost = 1500, Date = CurrentDate }
                    }
                },
                new Payment() { Id = 2, UserId = 1, CreditCardId = 1 },
                new Payment() { Id = 3, UserId = 1, CreditCardId = 1 },
                new Payment() { Id = 4, UserId = 2, CreditCardId = 1 }
            };
        }

        private List<CreditCard> CreateCreditCard()
        {
            return new List<CreditCard>()
            {
                new CreditCard() { Id = 1, UserId = 1, Name = "Primeiro Cartão" },
                new CreditCard() { Id = 2, UserId = 1, Name = "Segundo Cartão" }
            };
        }

        private List<User> CreateUserMock()
        {
            return new List<User>()
            {
                new User() { Id = 1, NickName = "Primeiro Usuário" },
                new User() { Id = 2, NickName = "Segundo Usuário" }
            };
        }

        private List<PaymentType> CreatePaymentTypesMock()
        {
            return new List<PaymentType>()
            {
                new PaymentType() { Id = 1, Description = "Despesa", In = false },
                new PaymentType() { Id = 2, Description = "Renda", In = true },
                new PaymentType() { Id = 3, Description = "Ganho", In = false },
                new PaymentType() { Id = 4, Description = "Crédito", In = true },
                new PaymentType() { Id = 5, Description = "Lucro", In = true },
            };
        }

        private List<Salary> CreateSalariesMock()
        {
            return new List<Salary>()
            {
                new Salary() { Id = 1, UserId = 1, StartDate = new DateTime(2019, 5, 1), EndDate = CreateEndDate(2019, 10), Value = 500 },
                new Salary() { Id = 2, UserId = 1, StartDate = new DateTime(2019, 11, 1), EndDate = CreateEndDate(2019, 12), Value = 800 },
                new Salary() { Id = 3, UserId = 1, StartDate = new DateTime(2020, 1, 1), EndDate = CreateEndDate(2020, 6), Value = 1200 },
                new Salary() { Id = 4, UserId = 1, StartDate = new DateTime(2020, 7, 1), EndDate = CreateEndDate(2020, 8), Value = 1500 },
                new Salary() { Id = 5, UserId = 1, StartDate = new DateTime(2020, 12, 1), EndDate = null, Value = 2000 },
                new Salary() { Id = 6, UserId = 2, StartDate = new DateTime(2019, 5, 1), EndDate = CreateEndDate(2019, 10), Value = 500 },
                new Salary() { Id = 7, UserId = 2, StartDate = new DateTime(2019, 11, 1), EndDate = CreateEndDate(2019, 12), Value = 800 },
                new Salary() { Id = 8, UserId = 2, StartDate = new DateTime(2020, 1, 1), EndDate = CreateEndDate(2020, 6), Value = 1200 },
                new Salary() { Id = 9, UserId = 2, StartDate = new DateTime(2020, 7, 1), EndDate = CreateEndDate(2020, 8), Value = 1500 },
                new Salary() { Id = 10, UserId = 2, StartDate = new DateTime(2020, 12, 1), EndDate = CreateEndDate(2021, 3), Value = 2000 },
                new Salary() { Id = 11, UserId = 3, StartDate = new DateTime(2020, 12, 1), EndDate = CreateEndDate(2021, 3), Value = 2000 },
            };
        }

        private List<DailyExpenses> CreateDailyExpensesMock()
        {
            return new List<DailyExpenses>()
            {
                new DailyExpenses()
                {
                    Id = 1,
                    UserId = 1,
                    ShopName = "Computer Shop",
                    Date = new DateTime(2019, 5, 1),
                    Items = new List<DailyExpensesItem> ()
                    {
                        new DailyExpensesItem() { DailyExpensesId = 1, ItemName = "Mouse", Price = 80 },
                        new DailyExpensesItem() { DailyExpensesId = 2, ItemName = "Processor", Price = 589.62M }
                    }
                }
            };
        }

        private List<Vehicle> CreateVehicleMock()
        {
            return new List<Vehicle>()
            {
                new Vehicle()
                {
                    Id = 1,
                    UserId = 1,
                    Description = "Moto",
                    FuelExpenses = new List<FuelExpenses> ()
                    {
                        new FuelExpenses() { Id = 1, Date = new DateTime(2020, 10,  1), Miliage = 100, PricePerLiter = 10, ValueSupplied = 30, VehicleId = 1 },
                        new FuelExpenses() { Id = 2, Date = new DateTime(2020, 10,  1), Miliage = 100, PricePerLiter = 10, ValueSupplied = 30, VehicleId = 1 },
                    }
                }
            };
        }

        private DateTime CreateEndDate(int year, int month) => new DateTime(year, month, DateTime.DaysInMonth(year, month));

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

        protected List<Vehicle> Vehicles
        {
            get
            {
                if (_vehicles == null)
                    _vehicles = CreateVehicleMock();
                return _vehicles;
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

        protected List<Salary> Salaries
        {
            get
            {
                if (_salaries == null)
                    _salaries = CreateSalariesMock();
                return _salaries;
            }
        }

        protected List<DailyExpenses> DailyExpenses
        {
            get
            {
                if (_dailyExpenses == null)
                    _dailyExpenses = CreateDailyExpensesMock();
                return _dailyExpenses;
            }
        }
    }
}
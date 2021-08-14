using System;
using Assignment_2.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Assignment_2.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<MCBAContext>();

            if (context.Customers.Any())
                return; 

            context.Customers.AddRange(

                new Customer
                {
                    CustomerID = 2100,
                    Name = "Matthew Bolger",
                    Address = "123 Fake Street",
                    Suburb = "Melbourne",
                    State = State.VIC,
                    Postcode = "3000"
                },
                new Customer
                {
                    CustomerID = 2200,
                    Name = "Rodney Cocker",
                    Address = "456 Real Road",
                    Suburb = "Melbourne",
                    State = State.VIC,
                    Postcode = "3000"
                },
                new Customer
                {
                    CustomerID = 2300,
                    Name = "Shekhar Kalra"
                });

            context.Logins.AddRange(

                new Login
                {
                    LoginID = "12345678",
                    CustomerID = 2100,
                    PasswordHash = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2",
                },
                new Login
                {
                    LoginID = "38074569",
                    CustomerID = 2200,
                    PasswordHash = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04",
                },
                new Login
                {
                    LoginID = "17963428",
                    CustomerID = 2300,
                    PasswordHash = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE",
                });

            context.Accounts.AddRange(

                new Account
                {
                    AccountNumber = 4100,
                    AccountType = AccountType.Saving,
                    CustomerID = 2100,
                    Balance = 500
                },
                new Account
                {
                    AccountNumber = 4101,
                    AccountType = AccountType.Checking,
                    CustomerID = 2100,
                    Balance = 500
                },
                new Account
                {
                    AccountNumber = 4200,
                    AccountType = AccountType.Saving,
                    CustomerID = 2200,
                    Balance = 500.95M
                },
                new Account
                {
                    AccountNumber = 4300,
                    AccountType = AccountType.Checking,
                    CustomerID = 2300,
                    Balance = 1250.50M
                });

            const string format = "dd/MM/yyyy hh:mm:ss tt";

            context.Transactions.AddRange(

                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = "Opening balance",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 10,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    AccountNumber = 4100,
                    Amount = 100,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 50,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 50,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    AccountNumber = 4100,
                    Amount = 10,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Transfer,
                    AccountNumber = 4100,
                    Amount = 100,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Transfer,
                    AccountNumber = 4100,
                    Amount = 50,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Transfer,
                    AccountNumber = 4100,
                    Amount = 200,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Transfer,
                    AccountNumber = 4100,
                    DestinationAccountNumber = 4101,
                    Amount = 200,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Transfer,
                    AccountNumber = 4100,
                    DestinationAccountNumber = 4101,
                    Amount = 50,

                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Transfer,
                    AccountNumber = 4100,
                    DestinationAccountNumber = 4101,
                    Amount = 100,

                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    AccountNumber = 4100,
                    Amount = 20,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Withdraw,
                    AccountNumber = 4100,
                    Amount = 50,
                    
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 200,
                    Comment = "First deposit",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:30:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 300,
                    Comment = "Second deposit",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 08:45:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 500,
                    Comment = "Deposited $500",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 09:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 0.95M,
                    Comment = "Deposited $0.95",
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 09:15:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4300,
                    Amount = 1250.50M,
                    TransactionTimeUtc = DateTime.ParseExact("19/05/2021 10:00:00 PM", format, null)
                });

            context.Payees.AddRange(

                new Payee
                {
                    Name = "Lawyer",
                    Address = "1 Pain Street",
                    Suburb = "Melbourne",
                    State = State.VIC,
                    Postcode = "3000",
                    Phone = "(03) 1234 1234"
                },
                new Payee
                {
                    Name = "Utilities",
                    Address = "1 Dont Care Blvd",
                    Suburb = "Melbourne",
                    State = State.VIC,
                    Postcode = "3000",
                    Phone = "(03) 0000 1234"
                },
                new Payee
                {
                    Name = "ATO",
                    Address = "1 Annoying Street",
                    Suburb = "Melbourne",
                    State = State.VIC,
                    Postcode = "3000",
                    Phone = "(03) 1111 1111"
                });

            context.SaveChanges();
        }
    }
}
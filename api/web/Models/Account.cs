using System.Collections.Generic;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public enum AccountType
    {
        Checking = 1,
        Saving = 2
    }
    public class Account
    {
        public const decimal WITHDRAW_CHARGE = 0.1M;
        public const decimal TRANSFER_CHARGE = 0.2M;
        public const decimal minCheckingBalance = 200;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Required, Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }

        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [DataType(DataType.Currency), Column(TypeName = "money")]
        public decimal Balance { get; set; }

        public virtual List<Transaction> Transactions { get; set; }

        public virtual List<BillPay> BillPays { get; set; }

        public void AddTransaction(TransactionType type, decimal amount, int destination = 0, string comment = null)
        {
            Transaction transaction = new Transaction()
            {
                TransactionType = type,
                AccountNumber = AccountNumber,
                Amount = amount,
                Comment = comment,
                TransactionTimeUtc = DateTime.UtcNow
            };

            if (destination == 0)
            {
                transaction.DestinationAccountNumber = null;
            }
            else
            {
                transaction.DestinationAccountNumber = destination;
            }

            Transactions.Add(transaction);
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
            AddTransaction(TransactionType.Deposit, amount);
        }

        public bool Withdraw(decimal amount)
        {
            bool sufficientFunds = CheckFunds(amount, WITHDRAW_CHARGE);

            if (!sufficientFunds)
            {
                return false;
            }

            var filteredList = Transactions.Where(t => t.TransactionType != TransactionType.ServiceCharge ||
            t.TransactionType != TransactionType.Deposit || t.TransactionType != TransactionType.BillPay);

            if (filteredList.Count() > 4)
            {
                Balance -= WITHDRAW_CHARGE;
                AddTransaction(TransactionType.ServiceCharge, WITHDRAW_CHARGE, 0);
            }

            Balance -= amount;
            AddTransaction(TransactionType.Withdraw, amount);

            return true;
        }

        public bool Transfer(decimal amount, Account destinationAccount, string comment)
        {
            bool sufficientFunds = CheckFunds(amount, TRANSFER_CHARGE);

            if (!sufficientFunds)
            {
                return false;
            }

            var filteredList = Transactions.Where(t => t.TransactionType != TransactionType.ServiceCharge ||
            t.TransactionType != TransactionType.Deposit || t.TransactionType != TransactionType.BillPay);

            if (filteredList.Count() > 4)
            {
                Balance -= TRANSFER_CHARGE;
                AddTransaction(TransactionType.ServiceCharge, TRANSFER_CHARGE, 0);
            }

            Balance -= amount;
            AddTransaction(TransactionType.Transfer, amount, destinationAccount.AccountNumber, comment);

            // Updating receiver account balance.
            destinationAccount.Balance = destinationAccount.Balance += amount;

            // Generate a transaction for receiver account.
            destinationAccount.AddTransaction(TransactionType.Transfer, amount, 0);

            return true;
        }

        public bool CheckFunds(decimal amount, decimal serviceCharge)
        {
            var filteredList = Transactions.Where(t => t.TransactionType != TransactionType.ServiceCharge ||
            t.TransactionType != TransactionType.Deposit || t.TransactionType != TransactionType.BillPay);

            if (filteredList.Count() > 4)
            {
                if (AccountType.Equals('S') && amount + serviceCharge > Balance)
                {
                    return false;
                }
                if (AccountType.Equals('C') && Balance - amount - serviceCharge < minCheckingBalance)
                {
                    return false;
                }
            }
            else
            {
                if (AccountType.Equals('S') && amount > Balance)
                {
                    return false;
                }
                if (AccountType.Equals('C') && Balance - amount < minCheckingBalance)
                {
                    return false;
                }
            }

            return true;
        }

        public void BillPay(BillPay billPay)
        {
            BillPay bill = new BillPay()
            {
                AccountNumber = AccountNumber,
                Amount = billPay.Amount,
                PayeeID = billPay.PayeeID,
                ScheduleTimeUtc = billPay.ScheduleTimeUtc,
                Period = billPay.Period
            };

            BillPays.Add(bill);
        }
    }
}


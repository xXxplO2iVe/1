using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Assignment_2.Data;
using Assignment_2.Models;
using X.PagedList;
using System.Collections.Generic;

namespace Assignment_2.Controllers
{
    public class StatementController : Controller
    {
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        private readonly MCBAContext _context;
        private const string AccountSessionKey = "_AccountSessionKey";

        public StatementController(MCBAContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View(customer);
        }

        
        public async Task<IActionResult> AccountStatement(int id)
        {
            {
                var account = await _context.Accounts.FindAsync(id);
                var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
                string accountJson = JsonConvert.SerializeObject(account, Formatting.Indented, serializerSettings);
                HttpContext.Session.SetString(AccountSessionKey, accountJson);
                return RedirectToAction(nameof(Statement));
            }
        }

        public async Task<IActionResult> Statement(int? page = 1)
        {
            var accountJson = HttpContext.Session.GetString(AccountSessionKey);

            if (accountJson == null)
                return RedirectToAction(nameof(Index));

            var account = JsonConvert.DeserializeObject<Account>(accountJson);
            ViewBag.Account = account;

            const int pageSize = 4;

            var pagedList = await _context.Transactions.Where(x => x.AccountNumber == account.AccountNumber).
                OrderByDescending(x => x.TransactionTimeUtc).ToPagedListAsync(page, pageSize);
            return View(pagedList);
        }

        public async Task<IActionResult> Graph(int id)
        {
            {
                var transactions = await _context.Transactions.Where(x => x.AccountNumber == id).ToListAsync<Transaction>();

                calcMoneyInOut(transactions);
                calcTransactionTypePercentage(transactions);

                return View();
            }
        }

        public void calcMoneyInOut(List<Transaction> transactions)
        {
            decimal moneyIn = 0;
            decimal moneyOut = 0;

            foreach (var transaction in transactions)
            {
                if (transaction.TransactionType == TransactionType.Deposit)
                {
                    moneyIn += transaction.Amount;
                }
                else if (transaction.TransactionType == TransactionType.Withdraw || transaction.TransactionType == TransactionType.BillPay
                    || transaction.TransactionType == TransactionType.ServiceCharge)
                {
                    moneyOut += transaction.Amount;
                }
                else if (transaction.TransactionType == TransactionType.Transfer && transaction.DestinationAccount == null)
                {
                    moneyIn += transaction.Amount;
                }
                else
                    moneyOut += transaction.Amount;
            }

            ViewBag.In = moneyIn;
            ViewBag.Out = moneyOut;
        }

        public void calcTransactionTypePercentage(List<Transaction> transactions)
        {
            decimal service = 0;
            decimal deposit = 0;
            decimal withdraw = 0;
            decimal transferIn = 0;
            decimal transferOut = 0;
            decimal billpay = 0;
            decimal total = 0;

            foreach (var transaction in transactions)
            {
                if (transaction.TransactionType == TransactionType.Deposit)
                {
                    deposit += transaction.Amount;
                }
                else if (transaction.TransactionType == TransactionType.Withdraw)
                {
                    withdraw += transaction.Amount;
                }
                else if (transaction.TransactionType == TransactionType.ServiceCharge)
                {
                    service += transaction.Amount;
                }
                else if (transaction.TransactionType == TransactionType.BillPay)
                {
                    billpay += transaction.Amount;
                }
                else if (transaction.TransactionType == TransactionType.Transfer && transaction.DestinationAccount == null)
                {
                    transferIn += transaction.Amount;
                }
                else
                    transferOut += transaction.Amount;
            }

            total = deposit + withdraw + service + billpay + transferIn + transferOut;

            ViewBag.Deposit = deposit / total;
            ViewBag.Withdraw = withdraw / total;
            ViewBag.Service = service / total;
            ViewBag.BillPay = billpay / total;
            ViewBag.TransferIn = transferIn / total;
            ViewBag.TransferOut = transferOut / total;
        }
    }
}

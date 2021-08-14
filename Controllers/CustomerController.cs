using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Assignment_2.Data;
using Assignment_2.Models;
using SimpleHashing;

namespace Assignment_2.Controllers
{
    public class CustomerController : Controller
    {
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        private readonly MCBAContext _context;

        public CustomerController(MCBAContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View(customer);
        }

        public async Task<IActionResult> Deposit(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        public IActionResult ConfirmDeposit(IFormCollection form)
        {
            ViewBag.AccountNumber = form["AccountNumber"];
            ViewBag.Amount = form["Amount"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(int accountNumber, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountNumber);

            if (amount <= 0)
            {
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            }

            if (Decimal.Round(amount, 2) != amount)
            {
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }

            account.Deposit(amount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Withdraw(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        public IActionResult ConfirmWithdraw(IFormCollection form)
        {
            ViewBag.AccountNumber = form["AccountNumber"];
            ViewBag.Amount = form["Amount"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(int accountNumber, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountNumber);

            if (amount <= 0)
            {
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            }

            if (Decimal.Round(amount, 2) != amount)
            {
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }

            bool transactionValid = account.Withdraw(amount);

            if (!transactionValid)
            {
                ModelState.AddModelError("Error", "Insufficient avaliable funds to service transaction.");
                return View(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Transfer(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        public IActionResult ConfirmTransfer(IFormCollection form)
        {
            ViewBag.AccountNumber = form["AccountNumber"];
            ViewBag.DestinationAccount = form["DestinationAccount"];
            ViewBag.Amount = form["Amount"];
            ViewBag.Comment = form["Comment"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(int accountNumber, int destinationAccount, decimal amount, string comment)
        {
            var account = await _context.Accounts.FindAsync(accountNumber);
            var destination = await _context.Accounts.FindAsync(destinationAccount);

            if (accountNumber == destinationAccount)
            {
                ModelState.AddModelError("Error", "Sender and destination account number can't be same.");
            }

            if (destination == null)
            {
                ModelState.AddModelError("DestinationAccount", "Invalid destination account number.");
            }

            if (amount <= 0)
            {
                ModelState.AddModelError("Amount", "Amount must be positive.");
            }

            if (Decimal.Round(amount, 2) != amount)
            {
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            }

            if (!ModelState.IsValid)
            {
                return View(account);
            }

            if (comment.Equals("") || comment.Equals("?"))
            {
                comment = null;
            }

            bool transactionValid = account.Transfer(amount, destination, comment);

            if (!transactionValid)
            {
                ModelState.AddModelError("Error", "Insufficient avaliable funds to service transaction.");
                return View(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateProfile() => View(await _context.Customers.FindAsync(CustomerID));

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Customer updateCustomer)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);

            if (ModelState.IsValid)
            {
                customer.Update(updateCustomer);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString(nameof(Customer.Name), updateCustomer.Name);
            }

            return View(customer);
        }

        public ViewResult ChangePassword() => View();

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var loginID = HttpContext.Session.GetString(nameof(Login.LoginID));
            var login = await _context.Logins.FindAsync(loginID);

            if (login == null || !PBKDF2.Verify(login.PasswordHash, currentPassword))
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new Login { LoginID = loginID });
            }

            login.PasswordHash = PBKDF2.Hash(newPassword);
            await _context.SaveChangesAsync();
            return View(nameof(ChangePassword));
        }
    }
}

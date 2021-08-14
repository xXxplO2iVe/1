using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using Web.Models;
using Newtonsoft.Json;
using System.Linq;

namespace Web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public TransactionController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public async Task<IActionResult> Index()
        {
            var response = await Client.GetAsync("api/account");
            var result = response.Content.ReadAsStringAsync().Result;
            var account = JsonConvert.DeserializeObject<List<Account>>(result);
           
            return View(account);
        }

        public async Task<IActionResult> History(int id)
        {
            var response = await Client.GetAsync($"api/account/{id}");
            var result = response.Content.ReadAsStringAsync().Result;
            var account = JsonConvert.DeserializeObject<Account>(result);
            IEnumerable<Transaction> transactions = account.Transactions.OrderByDescending(x => x.TransactionTimeUtc);
            ViewBag.AccountNumber = id;
            return View(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> History(int id, DateTime startDate, DateTime endDate)
        {
            var response = await Client.GetAsync($"api/account/{id}");
            var result = response.Content.ReadAsStringAsync().Result;
            var account = JsonConvert.DeserializeObject<Account>(result);
            IEnumerable<Transaction> transactions = account.Transactions.OrderByDescending(x => x.TransactionTimeUtc);
            var filter = FilterTransaction(transactions, startDate, endDate);
            ViewBag.AccountNumber = id;
            return View(filter);
        }

        public IEnumerable<Transaction> FilterTransaction(IEnumerable<Transaction> transactions, DateTime startDate, DateTime endDate)
        {
            List<Transaction> filter = transactions.ToList();

            if (endDate == DateTime.MinValue)
            {
                endDate = DateTime.MaxValue;
            }


            foreach (Transaction transaction in transactions)
            {
                if (transaction.TransactionTimeUtc.Date < startDate.Date || transaction.TransactionTimeUtc.Date > endDate.Date)
                {
                    filter.Remove(transaction);
                }
            }

            return filter;
        }


    }
}

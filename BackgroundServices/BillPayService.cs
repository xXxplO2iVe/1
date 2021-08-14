using System;
using System.Threading;
using System.Threading.Tasks;
using Assignment_2.Data;
using Assignment_2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Assignment_2.BackgroundServices
{
    public class BillPayService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<BillPayService> _logger;
        public BillPayService(IServiceProvider services, ILogger<BillPayService> logger)
        {
            _services = services;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Bill Pay service is running.");

            while (!cancellationToken.IsCancellationRequested)
            {
                await DoWork(cancellationToken);
                _logger.LogInformation("Bill Pay service is waiting a minute.");
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
        private async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Bill Pay service is working.");

            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MCBAContext>();
            var billPays = await context.BillPays.ToListAsync(cancellationToken);

            foreach (var billPay in billPays)
            {
                if (DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") == 
                    billPay.ScheduleTimeUtc.ToString("yyyy-MM-dd HH:mm"))
                {
                    await PayBillAsync(billPay, context).ConfigureAwait(false);
                }               
            }

            await context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Bill Pay service work complete.");
        }
        private async Task PayBillAsync(BillPay billPay, MCBAContext context)
        {
            var account = await context.Accounts.FindAsync(billPay.AccountNumber);

            if (account == null)
            {
                _logger.LogError($"Error! Invalid account number for Bill Pay (ID: {billPay.BillPayID}).");
                return;
            }

            if (billPay.Blocked==true)
            {
                _logger.LogInformation($"Error! Bill Pay (ID: {billPay.BillPayID}) is blocked by Admin.");
                return;
            }

            if (account.CheckFunds(billPay.Amount, 0))
            {
                account.BillPay(billPay.Amount);
                UpdateBillPay(billPay, context);
            }

            else
            {
                _logger.LogInformation($"Error! Customer account has insufficient funds to service Bill Pay (ID: {billPay.BillPayID}).");
                billPay.ScheduleTimeUtc = billPay.ScheduleTimeUtc.AddDays(1);
                return;
            }
        }
        private void UpdateBillPay(BillPay billPay, MCBAContext context)
        {
            if (billPay.Period == Period.OneOff)
            {
                context.BillPays.Remove(billPay);
                _logger.LogInformation($"Bill Pay (ID: {billPay.BillPayID}) has been processed and removed.");
            }

            if (billPay.Period == Period.Monthly)
            {
                billPay.ScheduleTimeUtc = billPay.ScheduleTimeUtc.AddMonths(1);
            }

            if (billPay.Period == Period.Quarterly)
            {
                billPay.ScheduleTimeUtc = billPay.ScheduleTimeUtc.AddMonths(3);
            }

            if (billPay.Period == Period.Annually)
            {
                billPay.ScheduleTimeUtc = billPay.ScheduleTimeUtc.AddYears(1);
            }

            _logger.LogInformation($"Bill Pay (ID: {billPay.BillPayID}) has been processed. " +
                    $"Next schedule time of bill {billPay.ScheduleTimeUtc}.");
        }
    }
}
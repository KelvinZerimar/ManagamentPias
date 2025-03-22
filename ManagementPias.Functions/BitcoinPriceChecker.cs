using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ManagementPias.Functions
{
    public class BitcoinPriceChecker
    {
        private readonly ILogger _logger;

        public BitcoinPriceChecker(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BitcoinPriceChecker>();
        }

        [Function("BitcoinPriceChecker")]
        public void Run([TimerTrigger("* * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}

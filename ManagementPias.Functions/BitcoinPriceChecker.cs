namespace ManagementPias.Functions
{
    //public class BitcoinPriceChecker
    //{
    //    private readonly HttpClient _httpClient;
    //    private readonly ILogger _logger;

    //    public BitcoinPriceChecker(IHttpClientFactory factory, ILoggerFactory loggerFactory)
    //    {
    //        _httpClient = factory.CreateClient(nameof(BitcoinPriceChecker));
    //        _logger = loggerFactory.CreateLogger<BitcoinPriceChecker>();
    //    }

    //    [Function("BitcoinPriceChecker")]
    //    public void Run([TimerTrigger("* * * * * *")] TimerInfo myTimer)
    //    {
    //        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

    //        //string apiResponse = _httpClient.GetStringAsync("https://api.coindesk.com/v1/bpi/currentprice/BTC.json").Result;
    //        //_logger.LogInformation($"Bitcoin price: {apiResponse}");

    //        if (myTimer.ScheduleStatus is not null)
    //        {
    //            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
    //        }
    //    }
    //}
}

namespace TradeMonkey.KuCoin.Function.Trigger.WebSocket
{
    public class KuCoinTickers
    {
        private readonly ILogger _logger;

        public KuCoinTickers(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("negotiate");
        }

        [Function("negotiate")]
        public HttpResponseData Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "Tickers")] MyConnectionInfo connectionInfo)
        {
            _logger.LogInformation($"SignalR Connection URL = '{connectionInfo.Url}'");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString($"Connection URL = '{connectionInfo.Url}'");

            return response;
        }
    }

    public class MyConnectionInfo
    {
        public string Url { get; set; }

        public string AccessToken { get; set; }
    }
}
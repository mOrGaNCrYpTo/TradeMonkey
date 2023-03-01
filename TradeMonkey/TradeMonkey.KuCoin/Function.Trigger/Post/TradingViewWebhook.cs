using TradeMonkey.KuCoin.Function.Domain.Value.Request;

namespace TradeMonkey.KuCoin.Function.Trigger.Post
{
    public class TradingViewWebhook
    {
        private readonly ILogger _logger;

        public TradingViewWebhook(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TradingViewWebhook>();
        }

        [Function(nameof(TradingViewWebhook))]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("WEBHOOK REQUEST RECEIVED");

            HttpResponseData functionResponse = req.CreateResponse(HttpStatusCode.OK);

            // Get the request body as a string
            string requestBody = new StreamReader(req.Body).ReadToEnd();

            // Parse the JSON request body into a TradingViewWebhookRequest object
            var request = JsonSerializer.Deserialize<TradingViewWebhookRequest>(requestBody);

            try
            {
                return functionResponse;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
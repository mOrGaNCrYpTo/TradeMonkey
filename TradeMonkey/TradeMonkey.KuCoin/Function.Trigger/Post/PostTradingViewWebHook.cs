namespace TradeMonkey.KuCoin.Trigger.Post
{
    public class PostTradingViewWebHook
    {
        private readonly ILogger _logger;

        public PostTradingViewWebHook(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PostTradingViewWebHook>();
        }

        [Function(nameof(PostTradingViewWebHook))]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("TradingView WebHook triggered");

            var response = req.CreateResponse(HttpStatusCode.OK);

            return response;
        }
    }
}
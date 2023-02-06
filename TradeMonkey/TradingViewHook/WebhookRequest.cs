namespace TradingViewHook
{
    public sealed class WebhookRequest
    {
        public string RequestType { get; set; }
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }
}

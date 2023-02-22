namespace TradeMonkey.KuCoin.Function.Domain.Value.Request
{
    public sealed class TradingViewWebhookRequest
    {
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public string Interval { get; set; }
        public List<double> Values { get; set; }
        public string Mode { get; set; }
        public string Webhook_id { get; set; }
    }
}
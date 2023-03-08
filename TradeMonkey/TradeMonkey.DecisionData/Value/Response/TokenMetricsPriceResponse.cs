using TradeMonkey.Data.Entity;

namespace TradeMonkey.Trader.Value.Response
{
    public sealed class TokenMetricsPriceResponse : BaseTokenMetricsResponse
    {
        [JsonPropertyName("data")]
        public List<TokenMetricsPrice> Data { get; set; } = new();
    }
}
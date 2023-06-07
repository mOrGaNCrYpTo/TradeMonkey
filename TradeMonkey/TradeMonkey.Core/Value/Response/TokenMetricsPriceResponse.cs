using TradeMonkey.Data.Entity;

namespace TradeMonkey.Core.Value.Response
{
    public sealed class TokenMetricsPriceResponse : BaseTokenMetricsResponse
    {
        [JsonPropertyName("data")]
        public List<TokenMetricsPrice> Data { get; set; } = new();
    }
}
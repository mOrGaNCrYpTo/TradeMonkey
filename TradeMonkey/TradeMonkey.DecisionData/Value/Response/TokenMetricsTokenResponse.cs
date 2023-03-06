using TradeMonkey.Data.Entity;

namespace TradeMonkey.Data.Value
{
    public class TokenMetricsTokenResponse : BaseTokenMetricsResponse
    {
        [JsonPropertyName("data")]
        public List<TokenMetricsToken> Data { get; set; } = new();
    }
}
namespace TradeMonkey.Core.Value.Response
{
    public class TokenMetricsTokenResponse : BaseTokenMetricsResponse
    {
        [JsonPropertyName("data")]
        public List<TokenMetricsToken> Data { get; set; } = new();
    }
}
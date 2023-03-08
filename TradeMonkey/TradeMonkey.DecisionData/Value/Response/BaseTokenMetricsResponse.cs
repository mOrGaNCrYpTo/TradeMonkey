namespace TradeMonkey.Trader.Value.Response
{
    public class BaseTokenMetricsResponse
    {
        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
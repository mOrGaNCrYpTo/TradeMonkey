namespace TradeMonkey.Core.Value.Response
{
    public sealed class TokenMetricsTraderGradesResponse : BaseTokenMetricsResponse
    {
        [JsonPropertyName("data")]
        public List<TraderGradesDatum> Data { get; set; } = new();
    }
}
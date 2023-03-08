using TradeMonkey.Data.Entity;

namespace TradeMonkey.Trader.Value.Response
{
    public sealed class TokenMetricsTraderGradesResponse : BaseTokenMetricsResponse
    {
        [JsonPropertyName("data")]
        public List<TraderGradesDatum> Data { get; set; } = new();
    }
}
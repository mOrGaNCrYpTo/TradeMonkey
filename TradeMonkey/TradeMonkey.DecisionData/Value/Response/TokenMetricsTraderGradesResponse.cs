using TradeMonkey.Data.Entity;

namespace TradeMonkey.DecisionData.Value.Response
{
    public sealed class TokenMetricsTraderGradesResponse : BaseTokenMetricsResponse
    {
        [JsonPropertyName("data")]
        public List<TraderGradesDatum> Data { get; set; } = new();
    }
}
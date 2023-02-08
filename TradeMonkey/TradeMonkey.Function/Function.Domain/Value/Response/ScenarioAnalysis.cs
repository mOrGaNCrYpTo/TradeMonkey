namespace TradeMonkey.TokenMetrics.Domain.Value.Response
{
    public class ScenarioAnalysis : Base
    {
        public ScenarioAnalysisData[] Data { get; set; }
    }

    public class ScenarioAnalysisData
    {
        public float CURRENT_DOMINANCE { get; set; }
        public string DATE { get; set; }
        public float DOMINANCE { get; set; }
        public int EPOCH { get; set; }
        public string NAME { get; set; }
        public float PREDICTION { get; set; }
        public string SYMBOL { get; set; }
        public int TOKEN_ID { get; set; }
        public float TOTAL_MARKET_CAP { get; set; }
    }
}
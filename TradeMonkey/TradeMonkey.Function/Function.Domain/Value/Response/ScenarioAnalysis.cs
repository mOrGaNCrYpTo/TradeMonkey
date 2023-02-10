namespace TradeMonkey.TokenMetrics.Domain.Value.Response
{
    public class ScenarioAnalysis : Base
    {
        public ScenarioAnalysisData[] Data { get; set; }
    }

    public class ScenarioAnalysisData
    {
        public float CurrentDominance { get; set; }
        public string Date { get; set; }
        public float Dominance { get; set; }
        public int Epoch { get; set; }
        public string Name { get; set; }
        public float Prediction { get; set; }
        public string Symbol { get; set; }
        public int TokenId { get; set; }
        public float TotalMarketCap { get; set; }
    }
}
namespace TradeMonkey.TokenMetrics.Domain.Value.Response
{
    public class QuantmetricsT1 : Base
    {
        public QuantmetricsT1Datum[] Data { get; set; }
    }

    public class QuantmetricsT1Datum
    {
        public float BETA { get; set; }
        public float CAGR { get; set; }
        public float CUMULATIVE_RETURN { get; set; }
        public string DATE { get; set; }
        public int END_PERIOD { get; set; }
        public int EPOCH { get; set; }
        public float MAX_DRAWDOWN { get; set; }
        public string NAME { get; set; }
        public float SHARPE { get; set; }
        public float SORTINO { get; set; }
        public int START_PERIOD { get; set; }
        public string SYMBOL { get; set; }
        public int TOKEN_ID { get; set; }
        public float VOLATILITY { get; set; }
    }
}
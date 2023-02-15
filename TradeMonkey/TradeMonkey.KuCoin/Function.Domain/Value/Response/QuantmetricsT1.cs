namespace TradeMonkey.KuCoin.Domain.Value.Response
{
    public class QuantmetricsT1 : Base
    {
        public QuantmetricsT1Datum[] Data { get; set; }
    }

    public class QuantmetricsT1Datum
    {
        public float Beta { get; set; }
        public float Cagr { get; set; }
        public float CumulativeReturn { get; set; }
        public string Date { get; set; }
        public int EndPeriod { get; set; }
        public int Epoch { get; set; }
        public float MaxDrawdown { get; set; }
        public string Name { get; set; }
        public float Sharpe { get; set; }
        public float Sortino { get; set; }
        public int StartPeriod { get; set; }
        public string Symbol { get; set; }
        public int TokenId { get; set; }
        public float Volatility { get; set; }
    }
}
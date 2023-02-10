namespace TradeMonkey.Data.Entity
{
    public class TradingIndicatorDatum
    {
        public float Close { get; set; }
        public string Date { get; set; }
        public int Epoch { get; set; }
        public float? HoldingCumulativeRoi { get; set; }
        public int LastSignal { get; set; }
        public string Name { get; set; }
        public int Signal { get; set; }
        public float? StrategyCummulativeRoi { get; set; }
        public string Symbol { get; set; }
        public int TokenId { get; set; }
        public float Volume { get; set; }
    }
}
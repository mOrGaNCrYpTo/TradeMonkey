namespace TradeMonkey.Function.Domain.Value.Response
{
    /// <summary>
    /// Get the AI generated trading signals for long and short positions
    /// </summary>

    public class TradingIndicator : Base
    {
        public TradingIndicatorDatum[] Data { get; set; }
    }

    public class TradingIndicatorDatum
    {
        public float CLOSE { get; set; }
        public string DATE { get; set; }
        public int EPOCH { get; set; }
        public float? HOLDING_CUMULATIVE_ROI { get; set; }
        public int LAST_SIGNAL { get; set; }
        public string NAME { get; set; }
        public int SIGNAL { get; set; }
        public float? STRATEGY_CUMULATIVE_ROI { get; set; }
        public string SYMBOL { get; set; }
        public int TOKEN_ID { get; set; }
        public float VOLUME { get; set; }
    }
}
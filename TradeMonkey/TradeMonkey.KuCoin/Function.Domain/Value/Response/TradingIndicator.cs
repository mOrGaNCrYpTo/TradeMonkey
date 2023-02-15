namespace TradeMonkey.KuCoin.Domain.Value.Response
{
    /// <summary>
    /// Get the AI generated trading signals for long and short positions
    /// </summary>

    public class TradingIndicator : Base
    {
        public TradingIndicatorDatum[] Data { get; set; }
    }
}
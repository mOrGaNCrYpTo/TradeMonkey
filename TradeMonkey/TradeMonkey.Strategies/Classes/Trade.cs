namespace TradeMonkey.Trader.Classes
{
    public enum TradeAction
    {
        Buy,
        Sell,
        Hold
    }

    public class TradeContext
    {
        public Symbol Symbol { get; set; } = new();
        public int Quantity { get; set; }
        public decimal StopLossPrice { get; set; }
        public decimal TakeProfitPrice { get; set; }
        public List<QuoteDto> HistoricalPrices { get; set; } = new();
        public List<QuoteDto> Quotes { get; set; } = new();
        public decimal CurrentPrice { get; set; }
    }
}
namespace TradeMonkey.Trader.Extensions
{
    public static class QuoteExtensions
    {
        public static decimal GetAtrSkender(this IEnumerable<IQuote> quotes, int period)
        {
            // Convert quotes to Candle format
            List<Candle> candles = quotes
                .Select(q => new Candle(q.Date, q.Open, q.High,
                                        q.Low, q.Close, q.Volume))
                .ToList();

            // Calculate ATR using Skender.Stock.Indicators
            AtrResult atr = candles.GetAtr(period).LastOrDefault();

            return (decimal)atr.Atr;
        }
    }
}
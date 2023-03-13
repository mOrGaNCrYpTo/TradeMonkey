using Skender.Stock.Indicators;

namespace TradeMonkey.Trader.Value.Aggregate
{
    public class Candle : IQuote
    {
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }

        public DateTime Date => Timestamp.Date;

        public Candle(DateTime timestamp, decimal open, decimal high, decimal low, decimal close, decimal volume)
        {
            Timestamp = timestamp;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public override string ToString()
        {
            return $"{Timestamp:yyyy-MM-dd HH:mm:ss.fff}, O:{Open:F2}, H:{High:F2}, L:{Low:F2}, C:{Close:F2}, V:{Volume:F2}";
        }
    }
}
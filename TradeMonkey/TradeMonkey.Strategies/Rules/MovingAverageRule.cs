namespace TradeMonkey.Trader.Rules
{
    public class MovingAveragesRule : ITradingRule
    {
        public int ShortPeriod { get; set; }
        public int LongPeriod { get; set; }

        public TradingSignal Signal => TradingSignal.GoLong;

        public async Task<TradingSignal> EvaluateAsync(List<QuoteDto> quotes, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var shortMa = TAIndicatorManager.GetSma(quotes, ShortPeriod);
            var longMa = TAIndicatorManager.GetSma(quotes, LongPeriod);

            if (shortMa < longMa)
            {
                return TradingSignal.GoLong;
            }
            else
            {
                return TradingSignal.GoShort;
            }
        }
    }
}
namespace TradeMonkey.Trader.Rules
{
    public sealed class MacdRule : ITradingRule
    {
        private readonly int _smaFastPeriods;
        private readonly int _smaSlowPeriods;
        private readonly int _signalPeriods;

        public TradingSignal CurrentSignal { get; private set; }

        public MacdRule(int smaFastPeriods, int smaSlowPeriods, int signalPeriods)
        {
            _smaFastPeriods = smaFastPeriods;
            _smaSlowPeriods = smaSlowPeriods;
            _signalPeriods = signalPeriods;
        }

        public async Task<TradingSignal> EvaluateAsync(List<QuoteDto> quotes, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            // Compute MACD and Signal Line
            var macd = TAIndicatorManager.GetMacd(quotes, FastPeriod, SlowPeriod);
            var signalLine = TAIndicatorManager.GetMacdSignalLine(quotes, FastPeriod, SlowPeriod, SignalPeriod);

            // Generate trading signals based on MACD crossovers
            if (macd > signalLine)
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
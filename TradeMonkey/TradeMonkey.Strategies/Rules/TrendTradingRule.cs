namespace TradeMonkey.Trader.Rules
{
    public sealed class TrendTradingRule : ITradingRule
    {
        private readonly int _smaFastPeriods;
        private readonly int _smaSlowPeriods;

        public TradingSignal Signal => TradingSignal.GoLong;

        public TrendTradingRule(int smaFastPeriods, int smaSlowPeriods)
        {
            _smaFastPeriods = smaFastPeriods;
            _smaSlowPeriods = smaSlowPeriods;
        }

        // or GoShort, depending on the rule

        public async Task<TradingSignal> EvaluateAsync(List<QuoteDto> quotes)
        {
            return await TAIndicatorManager.IsUptrend(quotes, _smaFastPeriods, _smaSlowPeriods) ?
                TradingSignal.GoLong : TradingSignal.GoShort;
        }
    }
}
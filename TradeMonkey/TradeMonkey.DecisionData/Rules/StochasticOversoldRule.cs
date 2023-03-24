namespace TradeMonkey.Trader.Rules
{
    public sealed class StochasticOversoldRule : ITradingRule
    {
        private readonly int _kPeriods;
        private readonly int _dPeriods;
        private readonly decimal _oversoldThreshold;

        public TradingSignal Signal => TradingSignal.GoLong;

        public StochasticOversoldRule(int kPeriods, int dPeriods, decimal oversoldThreshold)
        {
            _kPeriods = kPeriods;
            _dPeriods = dPeriods;
            _oversoldThreshold = oversoldThreshold;
        }

        public async Task<TradingSignal> EvaluateRuleSetAsync(List<QuoteDto> quotes)
        {
            //var stochasticValues = await TAIndicatorManager.GetStochasticValuesAsync(quotes, _kPeriods, _dPeriods);
            //return stochasticValues.Last().PercentK < _oversoldThreshold ? TradingSignal.GoLong : TradingSignal.None;
            return TradingSignal.GoLong;
        }
    }
}
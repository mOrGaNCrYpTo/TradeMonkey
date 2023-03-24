namespace TradeMonkey.Trader.Rules
{
    public sealed class StochasticOverboughtRule : ITradingRule
    {
        private readonly int _kPeriods;
        private readonly int _dPeriods;
        private readonly decimal _overboughtThreshold;

        public TradingSignal Signal => TradingSignal.GoShort;

        public StochasticOverboughtRule(int kPeriods, int dPeriods, decimal overboughtThreshold)
        {
            _kPeriods = kPeriods;
            _dPeriods = dPeriods;
            _overboughtThreshold = overboughtThreshold;
        }

        public async Task<TradingSignal> EvaluateRuleSetAsync(List<QuoteDto> quotes)
        {
            var stochasticValues = await TAIndicatorManager.GetStochasticRSI(quotes, _kPeriods, _dPeriods);
            return stochasticValues.Last().PercentK > _overboughtThreshold;
        }
    }
}
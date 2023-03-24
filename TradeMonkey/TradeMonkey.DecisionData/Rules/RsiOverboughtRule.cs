namespace TradeMonkey.Trader.Rules
{
    public sealed class RsiOverboughtRule : ITradingRule
    {
        private readonly int _rsiPeriods;
        private readonly double _overboughtThreshold;

        public TradingSignal Signal => TradingSignal.GoShort;

        public RsiOverboughtRule(int rsiPeriods, double overboughtThreshold)
        {
            _rsiPeriods = rsiPeriods;
            _overboughtThreshold = overboughtThreshold;
        }

        public async Task<TradingSignal> EvaluateRuleSetAsync(List<QuoteDto> quotes)
        {
            //return await TAIndicatorManager.IsOverbought(quotes, _rsiPeriods, _overboughtThreshold) ?
            //    TradingSignal.GoShort : TradingSignal.None;
            return TradingSignal.GoShort;
        }
    }
}
namespace TradeMonkey.Trader.Rules
{
    public sealed class RsiOversoldRule : ITradingRule
    {
        private readonly int _rsiPeriods;
        private readonly decimal _oversoldThreshold;

        public TradingSignal Signal => TradingSignal.GoLong;

        public RsiOversoldRule(int rsiPeriods, decimal oversoldThreshold)
        {
            _rsiPeriods = rsiPeriods;
            _oversoldThreshold = oversoldThreshold;
        }

        public async Task<TradingSignal> EvaluateRuleSetAsync(List<QuoteDto> quotes)
        {
            var rsiValues = await Task.Run(() => Indicator.GetRsi(quotes, _rsiPeriods));
            return (decimal)rsiValues.Last().Rsi < _oversoldThreshold ? TradingSignal.GoLong : TradingSignal.None;
        }
    }
}
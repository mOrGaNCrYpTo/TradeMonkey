namespace TradeMonkey.Trader.Rules
{
    public sealed class StochasticOverboughtRule : ITradingRule
    {
        private readonly int _rsiPeriods;
        private readonly int _signalPeriods;
        private readonly int _kPeriods;
        private readonly int _dPeriods;
        private readonly int _smoothPeriods;
        private readonly decimal _overboughtThreshold;

        // MACD properties
        public decimal MacdLine { get; set; }

        public TradingSignal Signal => TradingSignal.GoShort;

        public StochasticOverboughtRule(int rsiPeriods, int signalPeriods, int kPeriods, int dPeriods,
            int smoothPeriods, decimal overboughtThreshold)
        {
            _rsiPeriods = rsiPeriods;
            _signalPeriods = signalPeriods;
            _kPeriods = kPeriods;
            _dPeriods = dPeriods;
            _smoothPeriods = smoothPeriods;
            _overboughtThreshold = overboughtThreshold;
        }

        public async Task<TradingSignal> EvaluateRuleSetAsync(List<QuoteDto> quotes)
        {
            // Define parameters
            int rsiPeriods = 9;
            int stochPeriods = 7;
            int signalPeriods = 3;

            // Calculate StochRSI
            IEnumerable<StochRsiResult> results = quotes.GetStochRsi(
                rsiPeriods,
                stochPeriods,
                signalPeriods
            );

            var stochasticValue =
                TAIndicatorManager.IsStochasticOverbought(quotes, rsiPeriods, stochPeriods, _signalPeriods, _dPeriods);
            return stochasticValue > _overboughtThreshold ? TradingSignal.GoShort : TradingSignal.GoLong;
        }
    }
}
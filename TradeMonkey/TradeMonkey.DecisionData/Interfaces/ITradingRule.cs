namespace TradeMonkey.Trader.Interfaces
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

        public async Task<TradingSignal> EvaluateRuleSetAsync(List<QuoteDto> quotes)
        {
            return await TAIndicatorManager.IsUptrend(quotes, _smaFastPeriods, _smaSlowPeriods) ?
                TradingSignal.GoLong : TradingSignal.GoShort;
        }
    }

    public sealed class MacdTradingRule : ITradingRule
    {
        private readonly int _smaFastPeriods;
        private readonly int _smaSlowPeriods;
        private readonly int _signalPeriods;

        public TradingSignal Signal => TradingSignal.GoLong; // or GoShort, depending on the rule

        public MacdTradingRule(int smaFastPeriods, int smaSlowPeriods, int signalPeriods)
        {
            _smaFastPeriods = smaFastPeriods;
            _smaSlowPeriods = smaSlowPeriods;
            _signalPeriods = signalPeriods;
        }

        public async Task<TradingSignal> EvaluateRuleSetAsync(List<QuoteDto> quotes)
        {
            var macdResults = quotes.GetMacd(_smaFastPeriods, _smaSlowPeriods, _signalPeriods).ToList();
            var lastMacdResult = macdResults.Last();
            var prevMacdResult = macdResults[^2];

            return prevMacdResult.Macd < prevMacdResult.Signal && lastMacdResult.Macd > lastMacdResult.Signal ?
                TradingSignal.GoLong : TradingSignal.GoShort;
        }
    }

    public sealed class CrossoverAndAtaRule : ITradingRule
    {
        public int EmaFast { get; set; }
        public int EmaSlow { get; set; }
        public int SmmaFast { get; set; }
        public int SmmaMed { get; set; }
        public int SmmaSlow { get; set; }

        public TradingSignal Signal => TradingSignal.GoLong; // or GoShort, depending on the rule

        public async Task<TradingSignal> EvaluateRuleSetAsync(List<QuoteDto> quotes)
        {
            // Get the last quote and the previous quote
            var lastQuote = quotes.Last();
            var prevQuote = quotes[^2];

            Tuple<bool, bool, bool, bool> crossOverSignals =
                await TAIndicatorManager.GetEmaSmmaCrossoverSignals(quotes, lastQuote, EmaFast, EmaSlow,
                SmmaFast, SmmaMed, SmmaSlow);

            // ATR
            decimal atr = TAIndicatorManager.GetAtr(quotes, 14);

            // Assign points for each crossover signal and multiply the points awarded the ATR value
            // or reduce Assign points for each crossover signal, adjusted for ATR
            int goLongPoints =
                ((crossOverSignals.Item1 ? 1 : 0) * (int)Math.Floor(atr)) + ((crossOverSignals.Item3 ? 1 : 0) * (int)Math.Floor(atr * 0.5m));

            int goShortPoints =
                ((crossOverSignals.Item2 ? 1 : 0) * (int)Math.Floor(atr)) + ((crossOverSignals.Item4 ? 1 : 0) * (int)Math.Floor(atr * 0.5m));

            return goLongPoints > goShortPoints ? TradingSignal.GoLong : TradingSignal.GoShort;
        }
    }

    public sealed class RsiOversoldRule : ITradingRule
    {
        private readonly int _rsiPeriods;
        private readonly double _oversoldThreshold;

        public TradingSignal Signal => TradingSignal.GoLong;

        public RsiOversoldRule(int rsiPeriods, double oversoldThreshold)
        {
            _rsiPeriods = rsiPeriods;
            _oversoldThreshold = oversoldThreshold;
        }

        public async Task<TradingSignal> EvaluateRuleSetAsync(List<QuoteDto> quotes)
        {
            return await TAIndicatorManager.IsOversold(quotes, _rsiPeriods, _oversoldThreshold) ?
                TradingSignal.GoLong : TradingSignal.None;
        }
    }

    public interface ITradingRule
    {
        TradingSignal Signal { get; }

        Task<TradingSignal> EvaluateRuleSetAsync(List<IQuote> quotes);
    }
}
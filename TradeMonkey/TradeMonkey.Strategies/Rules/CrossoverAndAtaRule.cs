namespace TradeMonkey.Trader.Rules
{
    public sealed class CrossoverAndAtaRule : ITradingRule
    {
        public int EmaFast { get; set; }
        public int EmaSlow { get; set; }
        public int SmmaFast { get; set; }
        public int SmmaMed { get; set; }
        public int SmmaSlow { get; set; }

        public TradingSignal Signal => TradingSignal.GoLong;

        public async Task<TradingSignal> EvaluateAsync(List<QuoteDto> quotes, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var lastQuote = quotes.Last();
            var prevQuote = quotes[^2];

            var crossOverSignals = await TAIndicatorManager.GetEmaSmmaCrossoverSignals(quotes, lastQuote, EmaFast, EmaSlow,
                SmmaFast, SmmaMed, SmmaSlow);

            decimal atr = TAIndicatorManager.GetAtr(quotes, 14);

            int goLongPoints =
                ((crossOverSignals.Item1 ? 1 : 0) * (int)Math.Floor(atr)) + ((crossOverSignals.Item3 ? 1 : 0) * (int)Math.Floor(atr * 0.5m));

            int goShortPoints =
                ((crossOverSignals.Item2 ? 1 : 0) * (int)Math.Floor(atr)) + ((crossOverSignals.Item4 ? 1 : 0) * (int)Math.Floor(atr * 0.5m));

            return goLongPoints > goShortPoints ? TradingSignal.GoLong : TradingSignal.GoShort;
        }
    }
}
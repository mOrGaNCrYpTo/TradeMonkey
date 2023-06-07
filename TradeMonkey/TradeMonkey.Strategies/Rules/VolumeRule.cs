namespace TradeMonkey.Trader.Rules
{
    public class VolumeRule : ITradingRule
    {
        public TradingSignal Signal => TradingSignal.GoLong;

        public async Task<TradingSignal> EvaluateAsync(List<QuoteDto> quotes, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var lastQuote = quotes.Last();
            var prevQuote = quotes[^2];

            return lastQuote.Volume > prevQuote.Volume ? TradingSignal.GoLong : TradingSignal.GoShort;
        }
    }
}
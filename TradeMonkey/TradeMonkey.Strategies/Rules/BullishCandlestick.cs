namespace TradeMonkey.Trader.Rules
{
    /// <summary>
    /// This code checks for three types of super bullish candlestick patterns: a hammer, an
    /// engulfing pattern, and a piercing pattern. If any of these patterns occur, the EvaluateAsync
    /// method returns a GoLong signal. If not, it returns None.
    /// </summary>
    internal class BullishCandlestick : ITradingRule
    {
        public TradingSignal Signal => TradingSignal.None;

        public async Task<TradingSignal> EvaluateAsync(List<QuoteDto> quotes, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var lastQuote = quotes.Last();
            var prevQuote = quotes[^2];

            // Define a super bullish hammer: Close is at least 2% higher than Open and the lower
            // shadow is twice the body
            bool isSuperBullishHammer = lastQuote.Close > lastQuote.Open * 1.02m && (lastQuote.Open - lastQuote.Low) > 2 * (lastQuote.Close - lastQuote.Open);

            // Define a super bullish engulfing: Current candle's body engulfs the previous one and
            // is at least 2% of the price
            bool isSuperBullishEngulfing = lastQuote.Open < prevQuote.Close && lastQuote.Close > prevQuote.Open && (lastQuote.Close - lastQuote.Open) > lastQuote.Close * 0.02m;

            // Define a super bullish piercing: Current candle's body is at least 2% of the price
            // and it opens below the previous low but closes above the midpoint of the previous candle
            bool isSuperBullishPiercing = lastQuote.Open < prevQuote.Low && lastQuote.Close > (prevQuote.Open + prevQuote.Close) / 2 && (lastQuote.Close - lastQuote.Open) > lastQuote.Close * 0.02m;

            if (isSuperBullishHammer || isSuperBullishEngulfing || isSuperBullishPiercing)
            {
                return TradingSignal.GoLong;
            }
            else
            {
                return TradingSignal.None;
            }
        }
    }
}
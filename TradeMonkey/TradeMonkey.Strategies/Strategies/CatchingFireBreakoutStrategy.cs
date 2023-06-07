using TradeMonkey.Trader.Classes;

namespace TradeMonkey.Trader.Strategies
{
    public sealed class CatchingFireBreakoutStrategy : BaseChildStrategy
    {
        public CatchingFireBreakoutStrategy() : base(5) { }

        public override async Task<int> ExecuteStrategyAsync(TradeContext tradeContext)
        {
            // Calculate Bollinger Bands
            var bollingerBands = tradeContext.Quotes.GetBollingerBands(20).Last();

            // Get the last quote
            var lastQuote = tradeContext.Quotes.Last();

            // Check if the last quote price breaks above the upper band or below the lower band
            if (lastQuote.Close > (decimal?)bollingerBands.UpperBand)
            {
                // Check if the volume is higher than the average volume
                if (lastQuote.Volume > tradeContext.Quotes.Average(q => q.Volume))
                {
                    // It's a buy signal
                    return Weight;
                }
            }
            else if (lastQuote.Close < (decimal?)bollingerBands.LowerBand)
            {
                // Check if the volume is higher than the average volume
                if (lastQuote.Volume > tradeContext.Quotes.Average(q => q.Volume))
                {
                    // It's a sell signal
                    return -Weight;
                }
            }

            return 0;
        }
    }
}